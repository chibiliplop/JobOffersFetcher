using JobOffersFetcher.Core.Entities;

namespace JobOffersFetcher.Core.Repository;

public interface IFranceTravailApiRepository
{
    public Task<List<Offre>> GetOffreByCommune(int communeInseeCode);
    public Task<List<Offre>> GetOffreByDepartement(int departementInseeCode);
}