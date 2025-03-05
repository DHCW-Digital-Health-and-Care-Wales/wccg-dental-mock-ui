using Hl7.Fhir.Model;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Errors;

namespace WCCG.DentalMock.UI.Helpers;

public static class OperationOutcomeCreator
{
    public static OperationOutcome CreateOperationOutcome(OperationOutcome.IssueType issueType, BaseFhirHttpError error)
    {
        var issue = new OperationOutcome.IssueComponent
        {
            Severity = OperationOutcome.IssueSeverity.Error,
            Code = issueType,
            Details = new CodeableConcept(BaseFhirHttpError.System, error.Code, error.Display),
            Diagnostics = error.DiagnosticsMessage
        };

        return new OperationOutcome
        {
            Id = Guid.NewGuid().ToString(),
            Meta = new Meta { Profile = [FhirConstants.OperationOutcomeProfile] },
            Issue = [issue]
        };
    }
}
