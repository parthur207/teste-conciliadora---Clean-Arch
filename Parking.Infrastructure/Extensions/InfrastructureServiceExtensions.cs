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

        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS public.veiculo_historico (
                id uuid NOT NULL DEFAULT uuid_generate_v4(),
                veiculo_id uuid NOT NULL,
                cliente_id uuid NOT NULL,
                data_inicio timestamp with time zone NOT NULL,
                data_fim timestamp with time zone NULL,
                CONSTRAINT pk_veiculo_historico PRIMARY KEY (id),
                CONSTRAINT fk_vh_veiculo FOREIGN KEY (veiculo_id) REFERENCES public.veiculo(id) ON DELETE CASCADE,
                CONSTRAINT fk_vh_cliente FOREIGN KEY (cliente_id) REFERENCES public.cliente(id)
            )
        ");

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
