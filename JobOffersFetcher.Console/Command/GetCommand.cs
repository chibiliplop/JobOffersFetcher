using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Services;

namespace JobOffersFetcher.Console.Command;

[Command("get", Description = "Get offre by ID")]
public class GetCommand : ICommand
{
    private readonly OffreCrudService _offreCrudService;

    [CommandOption("id", Description = "Offre Id to get")]
    public required string Id { get; set; }

    public GetCommand(OffreCrudService offrecrudService)
    {
        _offreCrudService = offrecrudService;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        Offre offre = await _offreCrudService.GetOffreById(Id);
        if (offre == null)
        {
            throw new CommandException($"Offre with ID {Id} not found", -1);
        }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        console.Output.WriteLine(JsonSerializer.Serialize(offre, options));
    }
}