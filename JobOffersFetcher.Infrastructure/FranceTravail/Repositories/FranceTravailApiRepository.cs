using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Exception;
using JobOffersFetcher.Core.Repository;
using JobOffersFetcher.Infrastructure.FranceTravail.Configurations;
using JobOffersFetcher.Infrastructure.FranceTravail.Repositories.Interfaces;
using JobOffersFetcher.Infrastructure.FranceTravail.Response;
using Microsoft.Extensions.Logging;
using Models = JobOffersFetcher.Core.Entities;

namespace JobOffersFetcher.Infrastructure.FranceTravail.Repositories;

public class FranceTravailApiRepository : IFranceTravailApiRepository
{
    private readonly IFranceTravailAuth _auth;
    private readonly FranceTravailApiConfiguration _apiConfiguration;
    private readonly HttpClient _httpClient;
    private readonly ILogger<FranceTravailApiRepository> _logger;
    private const string PROVIDER = "FranceTravail";
    private SemaphoreSlim _semaphore;

    public FranceTravailApiRepository(IFranceTravailAuth franceTravailAuth, FranceTravailApiConfiguration apiConfiguration, HttpClient httpClient, ILogger<FranceTravailApiRepository> logger)
    {
        _auth = franceTravailAuth;
        _apiConfiguration = apiConfiguration;
        _httpClient = httpClient;
        _logger = logger;
        _semaphore = new SemaphoreSlim(apiConfiguration.MaxConcurrentRequests, apiConfiguration.MaxConcurrentRequests);
    }

    public Task<List<Offre>> GetOffreByCommune(int communeInseeCode)
    {
        NameValueCollection query = BuildCommuneQuery(communeInseeCode);
        return GetOffre(query);
    }

    public Task<List<Offre>> GetOffreByDepartement(int departementInseeCode)
    {
        NameValueCollection query = BuildDepartementQuery(departementInseeCode);
        return GetOffre(query);
    }

    public async Task<List<Offre>> GetOffre(NameValueCollection query)
    {
        bool isLastPage = true;
        List<Resultats> resultats = new List<Resultats>();
        int page = 0;
        do
        {
            ++page;
            (isLastPage, OffreSearchResponse response) = await FetchData(query, page);
            resultats.AddRange(response.resultats);
        } while (!isLastPage);

        return MapOffreSearchToOffre(resultats);
    }

    private List<Offre> MapOffreSearchToOffre(List<Resultats> response)
    {
        List<Offre> offres = new List<Offre>();
        foreach (var resultat in response)
        {
            offres.Add(new Offre
            {
                Id = resultat.id,
                DateCreation = resultat.dateCreation,
                DateActualisation = resultat.dateActualisation,
                Intitule = resultat.intitule,
                Description = resultat.description,
                TypeContrat = resultat.typeContrat,
                UrlPostulation = resultat.contact?.urlPostulation,
                Provider = PROVIDER,
                Entreprise = resultat.entreprise == null
                    ? null
                    : new Models.Entreprise
                    {
                        Nom = resultat.entreprise.nom,
                        Description = resultat.entreprise.description,
                        Logo = resultat.entreprise.logo,
                        Url = resultat.entreprise.url,
                        EntrepriseAdaptee = resultat.entreprise.entrepriseAdaptee
                    },
                LieuTravail = resultat.lieuTravail == null
                    ? null
                    : new Models.LieuTravail
                    {
                        Libelle = resultat.lieuTravail.libelle,
                        Latitude = resultat.lieuTravail.latitude,
                        Longitude = resultat.lieuTravail.longitude,
                        CodePostal = resultat.lieuTravail.codePostal,
                        Commune = resultat.lieuTravail.commune
                    }
            });
        }

        return offres;
    }

    private NameValueCollection BuildCommuneQuery(int communeInseeCode)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        query["commune"] = communeInseeCode.ToString();
        return query;
    }

    private NameValueCollection BuildDepartementQuery(int departementInseeCode)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        query["departement"] = departementInseeCode.ToString();
        return query;
    }

    private async Task<(bool, OffreSearchResponse)> FetchData(NameValueCollection query, int page = 1)
    {
        try
        {
            await _semaphore.WaitAsync();
            string requestUri = BuildRequestUri(query, page);
            string token = await _auth.GetToken();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            return await HandleResponse(response);
        }
        catch (Exception e) when (e is not JobOffersFetcherException)
        {
            _logger.LogError(e, "Exception while retrieving offre data from FranceTravailAPI");
            throw new InfrastructureException("Exception while retrieving offre data from FranceTravailAPI", e);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private string BuildRequestUri(NameValueCollection query, int page)
    {
        UriBuilder builder = new UriBuilder($"{_apiConfiguration.ApiBaseUrl}{FranceTravailApiConstant.OFFRES_SEARCH_PATH}");
        query["range"] = GetRange(page);
        builder.Query = query.ToString();
        return builder.ToString();
    }

    private string GetRange(int page)
    {
        return $"{page * FranceTravailApiConstant.OFFRES_SEARCH_DEFAULT_PAGE_SIZE - FranceTravailApiConstant.OFFRES_SEARCH_DEFAULT_PAGE_SIZE}-{Math.Min(FranceTravailApiConstant.OFFRES_SEARCH_DEFAULT_PAGE_SIZE * page, FranceTravailApiConstant.OFFRES_SEARCH_MAX_RANGE_SIZE)}";
    }

    private async Task<(bool, OffreSearchResponse)> HandleResponse(HttpResponseMessage response)
    {
        bool isLastPage = true;
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                break;
            case HttpStatusCode.PartialContent:
                if (response.Content.Headers.ContentRange != null)
                {
                    if (response.Content.Headers.ContentRange.Length > response.Content.Headers.ContentRange.To && response.Content.Headers.ContentRange.To < FranceTravailApiConstant.OFFRES_SEARCH_MAX_RANGE_SIZE)
                    {
                        isLastPage = false;
                    }
                }

                break;
            case HttpStatusCode.NotFound:
            case HttpStatusCode.NoContent:
                return (true, new OffreSearchResponse());
            default:
                _logger.LogError("Error while retrieving offre data for FranceTravailAPI: {StatusCode}", response.StatusCode);
                throw new InfrastructureException("Error while retrieving offre data for FranceTravailAPI");
        }

        await using (Stream responseStream = await response.Content.ReadAsStreamAsync())
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return (isLastPage, JsonSerializer.Deserialize<OffreSearchResponse>(responseStream, options));
        }
    }
}