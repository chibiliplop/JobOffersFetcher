namespace JobOffersFetcher.Core.Provider;

public interface IDateTimeProvider
{
    DateTime UtcNow => DateTime.UtcNow;
}