using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Services;

namespace JobOffersFetcher.Console.Command;

[Command("search", Description = "search offre by intitule")]
public class SearchCommand : ICommand
{
    private readonly OffreCrudService _offreCrudService;

    [CommandOption("searchterm", 's', Description = "search term to look for offre")]
    public required string SearchTerm { get; set; }

    public SearchCommand(OffreCrudService offreCrudService)
    {
        _offreCrudService = offreCrudService;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        List<Offre> offres = await _offreCrudService.SearchOffre(SearchTerm);
        if (offres.Count == 0)
        {
            console.Output.WriteLine($"No offre found with search term {SearchTerm}");
            return;
        }

        foreach (var offre in offres)
        {
            console.Output.WriteLine($"Offre ID: {offre.Id} - Intitule: {offre.Intitule}");
        }
    }
}