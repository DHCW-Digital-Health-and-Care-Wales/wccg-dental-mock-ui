using WCCG.DentalMock.UI.Constants;

namespace WCCG.DentalMock.UI.Errors;

public class UnexpectedExternalError : BaseFhirHttpError
{
    private readonly Exception _exception;

    public UnexpectedExternalError(Exception exception)
    {
        _exception = exception;
    }

    public override string Code => FhirHttpErrorCodes.ReceiverUnavailable;
    public override string DiagnosticsMessage => $"Unexpected error: {_exception.Message}";
}
