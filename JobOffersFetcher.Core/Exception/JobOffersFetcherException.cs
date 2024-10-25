namespace JobOffersFetcher.Core.Exception;

public class JobOffersFetcherException : System.Exception
{
    public JobOffersFetcherException()
    {
    }

    public JobOffersFetcherException(string message)
        : base(message)
    {
    }

    public JobOffersFetcherException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}