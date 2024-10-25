using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Repository;

namespace JobOffersFetcher.Core.Services;

public class StatisticServices
{
    private readonly IOffreRepository _offreRepository;

    public StatisticServices(IOffreRepository offreRepository)
    {
        _offreRepository = offreRepository;
    }


    public async Task<List<Statistic>> GetStatPerTypeContrat()
    {
        return await _offreRepository.GetStatPerTypeContrat();
    }

    public async Task<List<Statistic>> GetStatPerEntreprise()
    {
        return await _offreRepository.GetStatPerEntreprise();
    }

    public Task<List<Statistic>> GetStatPerCommune()
    {
        return _offreRepository.GetStatPerCommune();
    }
}