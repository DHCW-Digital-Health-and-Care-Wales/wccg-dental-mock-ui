using AutoFixture;
using FluentAssertions;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Errors;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;

namespace WCCG.DentalMock.UI.Unit.Tests.Errors;

public class UnexpectedInternalErrorTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();

    [Fact]
    public void ShouldCorrectlyCreateUnexpectedInternalError()
    {
        //Arrange
        var exception = _fixture.Create<Exception>();
        var expectedDetailsMessage = $"Unexpected error: {exception.Message}";
        const string expectedDisplayMessage = "500: The Receiver has encountered an error processing the request.";

        //Act
        var error = new UnexpectedInternalError(exception);

        //Assert
        error.Code.Should().Be(FhirHttpErrorCodes.ReceiverServerError);
        error.DiagnosticsMessage.Should().Be(expectedDetailsMessage);
        error.Display.Should().Be(expectedDisplayMessage);
    }
}
