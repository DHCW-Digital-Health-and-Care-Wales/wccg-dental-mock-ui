using AutoFixture;
using FluentAssertions;
using Hl7.Fhir.Model;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Errors;
using WCCG.DentalMock.UI.Helpers;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;

namespace WCCG.DentalMock.UI.Unit.Tests.Helpers;

public class OperationOutcomeCreatorTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();

    [Fact]
    public void CreateOperationOutcomeShouldCreateFromErrors()
    {
        //Arrange
        var issueType = _fixture.Create<OperationOutcome.IssueType>();
        var error = _fixture.Create<UnexpectedExternalError>();

        var expectedIssue = new OperationOutcome.IssueComponent
        {
            Severity = OperationOutcome.IssueSeverity.Error,
            Code = issueType,
            Details = new CodeableConcept(BaseFhirHttpError.System, error.Code, error.Display),
            Diagnostics = error.DiagnosticsMessage
        };

        //Act
        var result = OperationOutcomeCreator.CreateOperationOutcome(issueType, error);

        //Assert
        result.Id.Should().NotBeEmpty();
        result.Meta.Profile.Should().BeEquivalentTo(new List<string> { FhirConstants.OperationOutcomeProfile });
        result.Issue.Should().BeEquivalentTo([expectedIssue]);
    }
}
