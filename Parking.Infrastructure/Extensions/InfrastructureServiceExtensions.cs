using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parking.Application.IRepositories;
using Parking.Infrastructure.Persistence;
using Parking.Infrastructure.Repositories;

namespace Parking.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbContextParking>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IFaturaRepository, FaturaRepository>();
        services.AddScoped<IFaturaVeiculoRepository, FaturaVeiculoRepository>();
        services.AddScoped<IVeiculoHistoricoRepository, VeiculoHistoricoRepository>();

        return services;
    }

    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContextParking>();

        db.Database.Migrate();

        // Seed histórico para veículos que ainda não possuem registro ativo
        db.Database.ExecuteSqlRaw(@"
            INSERT INTO public.veiculo_historico (id, veiculo_id, cliente_id, data_inicio, data_fim)
            SELECT uuid_generate_v4(),
                   v.id,
                   v.cliente_id,
                   DATE_TRUNC('day', v.data_inclusao AT TIME ZONE 'UTC'),
                   NULL
            FROM public.veiculo v
            WHERE NOT EXISTS (
                SELECT 1 FROM public.veiculo_historico vh
                WHERE vh.veiculo_id = v.id AND vh.data_fim IS NULL
            )
        ");
    }
}
