using JobOffersFetcher.Core.Provider;
using JobOffersFetcher.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobOffersFetcher.Core;

public static class ServiceModule
{
    public static IServiceCollection RegisterCoreService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<FetchFranceTravailDataService, FetchFranceTravailDataService>()
            .AddSingleton<StatisticServices, StatisticServices>()
            .AddSingleton<OffreCrudService, OffreCrudService>();
        return services;
    }
}