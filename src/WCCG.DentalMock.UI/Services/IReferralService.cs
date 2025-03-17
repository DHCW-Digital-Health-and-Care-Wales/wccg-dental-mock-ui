namespace WCCG.DentalMock.UI.Services;

public interface IReferralService
{
    Task<HttpResponseMessage> CreateReferralAsync(string bundleJson, IHeaderDictionary headersDictionary);
    Task<HttpResponseMessage> GetReferralAsync(string referralId, IHeaderDictionary headersDictionary);
}
