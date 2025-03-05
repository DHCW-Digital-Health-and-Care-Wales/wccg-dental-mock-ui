using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using WCCG.DentalMock.UI.Configuration;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Extensions;

namespace WCCG.DentalMock.UI.Services;

public class ReferralService : IReferralService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReferralService> _logger;
    private readonly EReferralsApiConfig _eReferralsApiConfig;

    public ReferralService(HttpClient httpClient,
        IOptions<EReferralsApiConfig> eReferralsApiOptions,
        ILogger<ReferralService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _eReferralsApiConfig = eReferralsApiOptions.Value;
    }

    public async Task<HttpResponseMessage> CreateReferralAsync(string bundleJson, IHeaderDictionary headersDictionary)
    {
        try
        {
            AddHeaders(_httpClient, headersDictionary);

            return await _httpClient.PostAsync(_eReferralsApiConfig.CreateReferralEndpoint,
                new StringContent(bundleJson, new MediaTypeHeaderValue(FhirConstants.FhirMediaType)));
        }
        catch (Exception exception)
        {
            _logger.UnexpectedError(exception);
            throw;
        }
    }

    private static void AddHeaders(HttpClient client, IHeaderDictionary headersDictionary)
    {
        foreach (var header in RequestHeaderKeys.GetAll())
        {
            if (headersDictionary.TryGetValue(header, out var value))
            {
                client.DefaultRequestHeaders.Add(header, [value]);
            }
        }
    }
}
