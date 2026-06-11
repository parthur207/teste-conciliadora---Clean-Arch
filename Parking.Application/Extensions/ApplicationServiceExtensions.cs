using Microsoft.Extensions.DependencyInjection;
using Parking.Application.UseCases.Clientes;
using Parking.Application.UseCases.Faturas;
using Parking.Application.UseCases.Import;
using Parking.Application.UseCases.Veiculos;
using Parking.Domain.IUseCases.Clientes;
using Parking.Domain.IUseCases.Faturas;
using Parking.Domain.IUseCases.Import;
using Parking.Domain.IUseCases.Veiculos;

namespace Parking.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IListClientesUseCase, ListClientesUseCase>();
        services.AddScoped<ICreateClienteUseCase, CreateClienteUseCase>();
        services.AddScoped<IGetClienteByIdUseCase, GetClienteByIdUseCase>();
        services.AddScoped<IUpdateClienteUseCase, UpdateClienteUseCase>();
        services.AddScoped<IDeleteClienteUseCase, DeleteClienteUseCase>();

        services.AddScoped<IListVeiculosUseCase, ListVeiculosUseCase>();
        services.AddScoped<ICreateVeiculoUseCase, CreateVeiculoUseCase>();
        services.AddScoped<IGetVeiculoByIdUseCase, GetVeiculoByIdUseCase>();
        services.AddScoped<IUpdateVeiculoUseCase, UpdateVeiculoUseCase>();
        services.AddScoped<IDeleteVeiculoUseCase, DeleteVeiculoUseCase>();

        services.AddScoped<IGerarFaturaUseCase, GerarFaturaUseCase>();
        services.AddScoped<IListFaturasUseCase, ListFaturasUseCase>();
        services.AddScoped<IGetFaturaPlacasUseCase, GetFaturaPlacasUseCase>();

        services.AddScoped<IImportCsvUseCase, ImportCsvUseCase>();

        return services;
    }
}
