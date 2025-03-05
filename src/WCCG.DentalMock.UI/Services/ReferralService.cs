using System.Net.Http.Headers;
using System.Text.Json;
using Hl7.Fhir.Model;
using Microsoft.Extensions.Options;
using WCCG.DentalMock.UI.Configuration;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Exceptions;

namespace WCCG.DentalMock.UI.Services;

public class ReferralService : IReferralService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly EReferralsApiConfig _eReferralsApiConfig;

    public ReferralService(HttpClient httpClient,
        JsonSerializerOptions jsonSerializerOptions,
        IOptions<EReferralsApiConfig> eReferralsApiOptions)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = jsonSerializerOptions;
        _eReferralsApiConfig = eReferralsApiOptions.Value;
    }

    public async Task<string> CreateReferralAsync(string bundleJson, IHeaderDictionary headersDictionary)
    {
        AddHeaders(_httpClient, headersDictionary);

        var response = await _httpClient.PostAsync(_eReferralsApiConfig.CreateReferralEndpoint,
            new StringContent(bundleJson, new MediaTypeHeaderValue(FhirConstants.FhirMediaType)));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        var operationOutcome = await response.Content.ReadFromJsonAsync<OperationOutcome>(_jsonSerializerOptions);
        throw new ReferralCreationException(response.StatusCode, operationOutcome!);
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
