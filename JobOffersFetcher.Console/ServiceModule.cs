using JobOffersFetcher.Console.Command;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobOffersFetcher.Console;

public static class ServiceModule
{
    public static IServiceCollection RegisterCommand(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<FetchCommand, FetchCommand>()
            .AddSingleton<StatsCommand, StatsCommand>()
            .AddSingleton<GetCommand, GetCommand>()
            .AddSingleton<AddCommand, AddCommand>()
            .AddSingleton<SearchCommand, SearchCommand>();
        return services;
    }
}