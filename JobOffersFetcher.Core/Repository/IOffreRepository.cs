using JobOffersFetcher.Core.Entities;

namespace JobOffersFetcher.Core.Repository;

public interface IOffreRepository
{
    public Task<HashSet<Entreprise>> GetAllEntreprise();
    public Task<HashSet<Offre>> GetAllOffre();
    public Task AddOffre(Offre offre);
    public Task DeleteOffre(string offreId);
    public Task<Offre> GetOffreById(string offreId);
    public Task AddOffres(List<Offre> offres);
    public Task<List<Statistic>> GetStatPerTypeContrat();
    public Task<List<Statistic>> GetStatPerEntreprise();
    public Task<List<Statistic>> GetStatPerCommune();
    public Task<Entreprise> GetEntrepriseByName(string name);
    public Task<List<Offre>> SearchOffre(string searchTerm);
    Task UpdateOffres(List<Offre> offres);
}