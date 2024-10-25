using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Exception;
using JobOffersFetcher.Core.Repository;

namespace JobOffersFetcher.Core.Services;

public class OffreCrudService
{
    private readonly IOffreRepository _offreRepository;

    public OffreCrudService(IOffreRepository offreRepository)
    {
        _offreRepository = offreRepository;
    }

    public async Task AddOffre(Offre offre)
    {
        if ((await _offreRepository.GetOffreById(offre.Id)) != null)
        {
            throw new AlreadyExistException($"Offre with ID {offre.Id} already exists");
        }

        Entreprise entreprise = await _offreRepository.GetEntrepriseByName(offre.Entreprise.Nom);
        if (entreprise != null)
        {
            offre.Entreprise = entreprise;
        }

        await _offreRepository.AddOffre(offre);
    }
    
    public Task<List<Offre>> SearchOffre(string searchTerm)
    {
        return _offreRepository.SearchOffre(searchTerm);
    }

    public Task<Offre> GetOffreById(string offreId)
    {
        return _offreRepository.GetOffreById(offreId);
    }

    public Task DeleteOffreById(string offreId)
    {
        return _offreRepository.DeleteOffre(offreId);
    }
}