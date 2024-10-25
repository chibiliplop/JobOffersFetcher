namespace JobOffersFetcher.Infrastructure.FranceTravail.Configurations;

public record FranceTravailApiConfiguration
{
    public const string SectionName = "FranceTravailApi";
    public string ApiBaseUrl { get; set; }
    public string AuthBaseUrl { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public int MaxConcurrentRequests { get; set; } = 2;
}