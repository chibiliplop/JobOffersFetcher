using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Provider;
using JobOffersFetcher.Core.Services;

namespace JobOffersFetcher.Console.Command;

[Command("add", Description = "Add new job offer")]
public class AddCommand : ICommand
{
    private readonly OffreCrudService _offreCrudService;
    private readonly IDateTimeProvider _dateTimeProvider;

    [CommandOption("id", Description = "Offre Id default value is random GUID")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [CommandOption("intitule", Description = "Offre Intitule")]
    public required string Intitule { get; set; }

    [CommandOption("description", Description = "Offre Description")]
    public required string Description { get; set; }

    [CommandOption("typecontrat", Description = "Offre TypeContrat")]
    public required string TypeContrat { get; set; }

    [CommandOption("urlpostulation", Description = "Offre UrlPostulation")]
    public required string UrlPostulation { get; set; }

    [CommandOption("entreprisenom", Description = "Entreprise Nom")]
    public required string EntrepriseNom { get; set; }

    [CommandOption("entreprisedesc", Description = "Entreprise Description")]
    public string EntrepriseDescription { get; set; }

    [CommandOption("entrepriselogo", Description = "Entreprise Logo")]
    public string EntrepriseLogo { get; set; }

    [CommandOption("entrepriseurl", Description = "Entreprise Url")]
    public string EntrepriseUrl { get; set; }

    [CommandOption("lieutravailcp", Description = "Lieutravail Code Postal")]
    public string LieuTravailCodePostal { get; set; }

    [CommandOption("lieutravailcommune", Description = "Lieutravail Code insee commune")]
    public required string LieuTravailCommune { get; set; }

    [CommandOption("lieutravaillibelle", Description = "Lieutravail Libelee")]
    public string LieuTravailLibelle { get; set; }

    [CommandOption("lieuyravaillatitude", Description = "Lieutravail latitude")]
    public double LieuTravailLatitude { get; set; }

    [CommandOption("lieuyravaillongitude", Description = "Lieutravail longitude")]
    public double LieuTravailLongitude { get; set; }

    public AddCommand(OffreCrudService offreCrudService, IDateTimeProvider dateTimeProvider)
    {
        _offreCrudService = offreCrudService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        Offre offre = MapOffre();
        await _offreCrudService.AddOffre(offre);
        console.Output.WriteLine($"Offre with ID {offre.Id} added");
    }

    public Offre MapOffre()
    {
        return new Offre
        {
            Id = this.Id,
            Intitule = this.Intitule,
            Description = this.Description,
            TypeContrat = this.TypeContrat,
            UrlPostulation = this.UrlPostulation,
            DateCreation = _dateTimeProvider.UtcNow,
            DateActualisation = _dateTimeProvider.UtcNow,
            Provider = "JobOffers",
            Entreprise = new Entreprise
            {
                Nom = this.EntrepriseNom,
                Description = this.EntrepriseDescription,
                Logo = this.EntrepriseLogo,
                Url = this.EntrepriseUrl
            },
            LieuTravail = new LieuTravail
            {
                CodePostal = this.LieuTravailCodePostal,
                Commune = this.LieuTravailCommune,
                Libelle = this.LieuTravailLibelle,
                Latitude = this.LieuTravailLatitude,
                Longitude = this.LieuTravailLongitude
            }
        };
    }
}