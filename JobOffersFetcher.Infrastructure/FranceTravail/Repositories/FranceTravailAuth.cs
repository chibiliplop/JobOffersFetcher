using System.Net;
using System.Text.Json;
using JobOffersFetcher.Core.Exception;
using JobOffersFetcher.Core.Provider;
using JobOffersFetcher.Infrastructure.FranceTravail.Configurations;
using JobOffersFetcher.Infrastructure.FranceTravail.Repositories.Interfaces;
using JobOffersFetcher.Infrastructure.FranceTravail.Response;
using Microsoft.Extensions.Logging;

namespace JobOffersFetcher.Infrastructure.FranceTravail.Repositories;

public class FranceTravailAuth : IFranceTravailAuth
{
    private FranceTravailApiConfiguration apiConfiguration;

    private Task<string> _tokenTask;
    private DateTime _nextExpiration;
    private readonly object _lockObj = new();
    private readonly ILogger<FranceTravailAuth> _logger;
    private readonly FranceTravailApiConfiguration _apiConfiguration;
    private IDateTimeProvider _dateTimeProvider;


    public FranceTravailAuth(FranceTravailApiConfiguration apiConfiguration, IDateTimeProvider dateTimeProvider, ILogger<FranceTravailAuth> logger)
    {
        _apiConfiguration = apiConfiguration;
        _logger = logger;
        _nextExpiration = DateTime.MinValue;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task<string> GetToken()
    {
        lock (_lockObj)
        {
            if (IsTokenExpired())
            {
                TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
                _tokenTask = tcs.Task;
                //Never await in this function this will cause deadlock and performance issue
                return QueryToken().ContinueWith(t =>
                {
                    lock (_lockObj)
                    {
                        if (t.IsFaulted)
                        {
                            tcs.SetException(t.Exception);
                            throw t.Exception;
                        }

                        if (t.IsCanceled)
                        {
                            tcs.SetCanceled();
                            throw new OperationCanceledException();
                        }

                        tcs.SetResult(t.Result.access_token);
                        _nextExpiration = DateTime.UtcNow.AddSeconds(t.Result.expires_in - FranceTravailApiConstant.TOKEN_RENEW_MARGIN_SECONDS);
                    }

                    return t.Result.access_token;
                });
            }
        }

        return _tokenTask;
    }

    private bool IsTokenExpired()
    {
        return _nextExpiration <= _dateTimeProvider.UtcNow;
    }

    private async Task<FranceTravailAuthToken> QueryToken()
    {
        try
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_apiConfiguration.AuthBaseUrl}{FranceTravailApiConstant.ACCESS_TOKEN_PATH}");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _apiConfiguration.ClientId },
                { "client_secret", _apiConfiguration.ClientSecret },
                { "scope", FranceTravailApiConstant.TOKEN_SCOPE }
            });
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Error while generating AuthToken for FranceTravailAPI: {StatusCode}", response.StatusCode);
                throw new InfrastructureException("Error while generating AuthToken for FranceTravailAPI");
            }

            Stream responseContent = await response.Content.ReadAsStreamAsync();
            FranceTravailAuthToken? token = JsonSerializer.Deserialize<FranceTravailAuthToken>(responseContent);
            return token;
        }
        catch (Exception e) when (e is not JobOffersFetcherException)
        {
            _logger.LogError(e, "Exception while generating AuthToken for FranceTravailAPI");
            throw new InfrastructureException("Exception while generating AuthToken for FranceTravailAPI", e);
        }
    }
}