namespace JobOffersFetcher.Core.Exception;

public class AlreadyExistException : JobOffersFetcherException
{
    public AlreadyExistException()
    {
    }

    public AlreadyExistException(string message)
        : base(message)
    {
    }

    public AlreadyExistException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}