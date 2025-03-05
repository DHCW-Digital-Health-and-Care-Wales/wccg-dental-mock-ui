namespace WCCG.DentalMock.UI.Services;

public interface IReferralService
{
    Task<string> CreateReferralAsync(string bundleJson, IHeaderDictionary headersDictionary);
}
