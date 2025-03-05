using WCCG.DentalMock.UI.Constants;

namespace WCCG.DentalMock.UI.Errors;

public class UnexpectedInternalError : BaseFhirHttpError
{
    private readonly Exception _exception;

    public UnexpectedInternalError(Exception exception)
    {
        _exception = exception;
    }

    public override string Code => FhirHttpErrorCodes.ReceiverServerError;
    public override string DiagnosticsMessage => $"Unexpected error: {_exception.Message}";
}
