using CliFx;
using JobApi.Infrastructure;
using JobOffersFetcher.Core;
using JobOffersFetcher.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JobOffersFetcher.Console;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<JobOffersFetcherContext>()
            .UseSqlite(configuration.GetConnectionString("JobOfferConnection"));
        var context = new JobOffersFetcherContext(optionsBuilder.Options);
        context.Database.EnsureCreated();

        var services = new ServiceCollection()
            .AddLogging(loggingBuilder => { loggingBuilder.AddConsole(); })
            .AddSingleton(configuration)
            .AddSingleton(optionsBuilder.Options)
            .AddHttpClient()
            .RegisterCoreService(configuration)
            .RegisterInfrastructureServices(configuration)
            .RegisterCommand(configuration)
            .AddDbContextPool<JobOffersFetcherContext>(options => options.UseSqlite(configuration.GetConnectionString("JobOfferConnection")))
            .BuildServiceProvider();

        return await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(services.GetService)
            .Build()
            .RunAsync();
    }
}