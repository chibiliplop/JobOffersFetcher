namespace JobOffersFetcher.Infrastructure.FranceTravail.Repositories.Interfaces;

public interface IFranceTravailAuth
{
    public Task<string> GetToken();
}