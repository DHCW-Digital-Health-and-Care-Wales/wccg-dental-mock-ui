using System.Net;
using AutoFixture;
using FluentAssertions;
using Hl7.Fhir.Model;
using WCCG.DentalMock.UI.Errors;
using WCCG.DentalMock.UI.Exceptions;
using WCCG.DentalMock.UI.Helpers;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;

namespace WCCG.DentalMock.UI.Unit.Tests.Exceptions;

public class ReferralCreationExceptionTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();

    [Fact]
    public void ShouldCorrectlyCreateReferralCreationException()
    {
        //Arrange
        var exception = _fixture.Create<Exception>();
        var operationOutcome =
            OperationOutcomeCreator.CreateOperationOutcome(OperationOutcome.IssueType.Transient, new UnexpectedExternalError(exception));

        var statusCode = _fixture.Create<HttpStatusCode>();

        //Act
        var resultException = new ReferralCreationException(statusCode, operationOutcome);

        //Assert
        resultException.StatusCode.Should().Be(statusCode);
        resultException.OperationOutcome.Should().BeEquivalentTo(operationOutcome);
        exception.Message.Should().Contain(exception.Message);
    }
}
