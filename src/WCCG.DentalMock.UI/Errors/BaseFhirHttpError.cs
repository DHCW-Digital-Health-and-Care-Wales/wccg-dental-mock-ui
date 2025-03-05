using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Helpers;

namespace WCCG.DentalMock.UI.Errors;

public abstract class BaseFhirHttpError
{
    public static string System => FhirConstants.HttpErrorCodesSystem;
    public string Display => FhirHttpErrorHelper.GetDisplayMessageByCode(Code);
    public abstract string Code { get; }
    public abstract string DiagnosticsMessage { get; }
}
