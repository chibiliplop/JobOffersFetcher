using JobOffersFetcher.Core.Repository;
using JobOffersFetcher.Infrastructure;
using JobOffersFetcher.Infrastructure.FranceTravail.Configurations;
using JobOffersFetcher.Infrastructure.FranceTravail.Repositories;
using JobOffersFetcher.Infrastructure.FranceTravail.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JobApi.Infrastructure;

public static class ServiceModule
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FranceTravailApiConfiguration>(configuration.GetSection(FranceTravailApiConfiguration.SectionName));
        services.AddSingleton(config => config.GetRequiredService<IOptions<FranceTravailApiConfiguration>>().Value);
        services.AddSingleton<IFranceTravailAuth, FranceTravailAuth>();
        services.AddScoped<IFranceTravailApiRepository, FranceTravailApiRepository>();
        services.AddScoped<IOffreRepository, OffreRepository>();
        return services;
    }
}