using CdbRadar.Application.Abstractions;
using CdbRadar.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace CdbRadar.Application.DependencyInjection;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOfertaUseCase, OfertaUseCase>();

        return services;
    }
}
