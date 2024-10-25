using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using JobOffersFetcher.Core.Services;

namespace JobOffersFetcher.Console.Command;

[Command("fetch", Description = "Fetch the job offers incrementally from FranceTravail API")]
public class FetchCommand : ICommand
{
    private readonly FetchFranceTravailDataService _fetchServices;

    [CommandOption("commune", 'c', Description = "Commune code insee can be renseigned multiple times")]
    public List<int> communes { get; set; } = new List<int>();

    [CommandOption("departement", 'd', Description = "departement code insee can be renseigned multiple times")]
    public List<int> departement { get; set; } = new List<int>();
    
    public FetchCommand(FetchFranceTravailDataService fetchServices)
    {
        _fetchServices = fetchServices;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        if(communes.Count == 0 && departement.Count == 0)
        {
            throw new CommandException("Please provide at least one commune or one departement", -1);
        }
        await _fetchServices.FetchData(communes, departement);
    }
}