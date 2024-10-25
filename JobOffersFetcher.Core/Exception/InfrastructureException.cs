namespace JobOffersFetcher.Core.Exception;

public class InfrastructureException : JobOffersFetcherException
{
    public InfrastructureException()
    {
    }

    public InfrastructureException(string message)
        : base(message)
    {
    }

    public InfrastructureException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}