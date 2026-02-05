using CdbRadar.Repository.Interfaces;
using CdbRadar.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CdbRadar.Repository.DependencyInjection
{
    public static class RepositoryModule
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOfertaRepository, OfertaRepository>();
            services.AddScoped<ITipoAtivoRepository, TipoAtivoRepository>();
            return services;
        }
    }
}
