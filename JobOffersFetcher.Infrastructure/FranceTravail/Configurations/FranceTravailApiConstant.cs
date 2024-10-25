namespace JobOffersFetcher.Infrastructure.FranceTravail.Configurations;

internal static class FranceTravailApiConstant
{
    public const string ACCESS_TOKEN_PATH = "/connexion/oauth2/access_token?realm=%2Fpartenaire";
    public const string OFFRES_SEARCH_PATH = "/partenaire/offresdemploi/v2/offres/search";
    public const int TOKEN_RENEW_MARGIN_SECONDS = 10;
    public const string TOKEN_SCOPE = "o2dsoffre api_offresdemploiv2";
    public const int OFFRES_SEARCH_MAX_RANGE_SIZE = 3000;
    public const int OFFRES_SEARCH_DEFAULT_PAGE_SIZE = 100;
}