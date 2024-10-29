using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Provider;
using JobOffersFetcher.Core.Repository;
using Microsoft.Extensions.Logging;

namespace JobOffersFetcher.Core.Services;

public class FetchFranceTravailDataService
{
    private readonly IFranceTravailApiRepository _franceTravailApiRepository;
    private readonly IOffreRepository _offreRepository;
    private readonly ILogger<FetchFranceTravailDataService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public FetchFranceTravailDataService(IFranceTravailApiRepository franceTravailApiRepository, IOffreRepository offreRepository, ILogger<FetchFranceTravailDataService> logger, IDateTimeProvider dateTimeProvider)
    {
        _franceTravailApiRepository = franceTravailApiRepository;
        _offreRepository = offreRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task FetchData(List<int> commune, List<int> departement)
    {
        _logger.LogInformation("Fetching data from FranceTravail API Start {Now}", _dateTimeProvider.UtcNow);
        List<Offre> offres = await CrawlData(commune, departement);
        await IngestData(offres);
        _logger.LogInformation("Fetching data from FranceTravail API End {Now}", _dateTimeProvider.UtcNow);
    }

    private async Task<List<Offre>> CrawlData(List<int> commune, List<int> departement)
    {
        List<Task<List<Offre>>> tasks = new List<Task<List<Offre>>>();
        foreach (int communeInseeCode in commune)
        {
            tasks.Add(_franceTravailApiRepository.GetOffreByCommune(communeInseeCode));
        }

        foreach (int departementInseeCode in departement)
        {
            tasks.Add(_franceTravailApiRepository.GetOffreByDepartement(departementInseeCode));
        }

        var result = await Task.WhenAll(tasks);
        List<Offre> offres = result.SelectMany(x => x).DistinctBy(offre => offre.Id).ToList();
        _logger.LogInformation("Crawler get {NumberDistinctOffre} at {_dateTimeProvider.Now}", offres.Count, _dateTimeProvider.UtcNow);
        return offres;
    }
    

    private async Task IngestData(List<Offre> offres)
    {
        List<Offre> offreToIngest = new List<Offre>();
        List<Offre> offreToUpdate = new List<Offre>();
        HashSet<Entreprise> existingEntreprises = await _offreRepository.GetAllEntreprise();
        HashSet<Offre> existingOffreIds = await _offreRepository.GetAllOffre();
        foreach (Offre offre in offres)
        {
            if (existingOffreIds.TryGetValue(offre, out Offre existingOffre))
            {
                if(existingOffre.DateActualisation > offre.DateActualisation)
                {
                    offre.Entreprise = GetOrCreateEntreprise(existingEntreprises, offre.Entreprise);
                    offreToUpdate.Add(offre);
                }
                continue;
            }

            if (offre.LieuTravail.Commune == null)
            {
                offre.LieuTravail.Commune = "Commune non renseignée";
            }

            offre.Entreprise = GetOrCreateEntreprise(existingEntreprises, offre.Entreprise);
            offreToIngest.Add(offre);
        }

        await _offreRepository.AddOffres(offreToIngest);
        _logger.LogInformation("Ingester added {offreToIngest} at {_dateTimeProvider.Now}", offreToIngest.Count, _dateTimeProvider.UtcNow);
        await _offreRepository.UpdateOffres(offreToUpdate);
        _logger.LogInformation("Ingester update {offreToUpdate} at {_dateTimeProvider.Now}", offreToUpdate.Count, _dateTimeProvider.UtcNow);

    }

    private Entreprise GetOrCreateEntreprise(HashSet<Entreprise> existingEntreprises, Entreprise offreEntreprise)
    {
        if (offreEntreprise.Nom == null)
        {
            offreEntreprise.Nom = "Entreprise non renseignée";
        }

        if (!existingEntreprises.TryGetValue(offreEntreprise, out Entreprise existingentreprise))
        {
            existingentreprise = offreEntreprise;
            existingEntreprises.Add(existingentreprise);
        }

        return existingentreprise;
    }
}