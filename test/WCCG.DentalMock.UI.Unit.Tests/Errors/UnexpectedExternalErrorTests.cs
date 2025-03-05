using AutoFixture;
using FluentAssertions;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Errors;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;

namespace WCCG.DentalMock.UI.Unit.Tests.Errors;

public class UnexpectedExternalErrorTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();

    [Fact]
    public void ShouldCorrectlyCreateUnexpectedExternalError()
    {
        //Arrange
        var exception = _fixture.Create<Exception>();
        var expectedDetailsMessage = $"Unexpected error: {exception.Message}";
        const string expectedDisplayMessage = "503: The Receiver is currently unavailable.";

        //Act
        var error = new UnexpectedExternalError(exception);

        //Assert
        error.Code.Should().Be(FhirHttpErrorCodes.ReceiverUnavailable);
        error.DiagnosticsMessage.Should().Be(expectedDetailsMessage);
        error.Display.Should().Be(expectedDisplayMessage);
    }
}
