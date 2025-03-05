using System.Net;
using Hl7.Fhir.Model;

namespace WCCG.DentalMock.UI.Exceptions;

public class ReferralCreationException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public OperationOutcome OperationOutcome { get; }

    public ReferralCreationException(HttpStatusCode statusCode, OperationOutcome operationOutcome)
    {
        StatusCode = statusCode;
        OperationOutcome = operationOutcome;
    }

    public override string Message => OperationOutcome.Issue.Select(x => x.Diagnostics).Aggregate((f, s) => string.Join(';', f, s));
}
