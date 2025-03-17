using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using WCCG.DentalMock.UI.Configuration;
using WCCG.DentalMock.UI.Constants;

namespace WCCG.DentalMock.UI.Services;

public class ReferralService : IReferralService
{
    private readonly HttpClient _httpClient;
    private readonly EReferralsApiConfig _eReferralsApiConfig;

    public ReferralService(HttpClient httpClient, IOptions<EReferralsApiConfig> eReferralsApiOptions)
    {
        _httpClient = httpClient;
        _eReferralsApiConfig = eReferralsApiOptions.Value;
    }

    public async Task<HttpResponseMessage> CreateReferralAsync(string bundleJson, IHeaderDictionary headersDictionary)
    {
        AddHeaders(_httpClient, headersDictionary);

        return await _httpClient.PostAsync(_eReferralsApiConfig.CreateReferralEndpoint,
            new StringContent(bundleJson, new MediaTypeHeaderValue(FhirConstants.FhirMediaType)));
    }

    public async Task<HttpResponseMessage> GetReferralAsync(string referralId, IHeaderDictionary headersDictionary)
    {
        AddHeaders(_httpClient, headersDictionary);

        var endpoint = string.Format(CultureInfo.InvariantCulture, _eReferralsApiConfig.GetReferralEndpoint, referralId);
        return await _httpClient.GetAsync(endpoint);
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
