using AutoFixture;
using FluentAssertions;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Helpers;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;

namespace WCCG.DentalMock.UI.Unit.Tests.Helpers;

public class FhirHttpErrorHelperTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();

    [Theory]
    [InlineData(FhirHttpErrorCodes.ReceiverUnavailable, "503: The Receiver is currently unavailable.")]
    [InlineData(FhirHttpErrorCodes.ReceiverServerError, "500: The Receiver has encountered an error processing the request.")]
    public void GetDisplayMessageByCodeShouldReturnCorrectMessages(string code, string expectedMessage)
    {
        //Act
        var message = FhirHttpErrorHelper.GetDisplayMessageByCode(code);

        //Assert
        message.Should().Be(expectedMessage);
    }

    [Fact]
    public void GetDisplayMessageByCodeShouldReturnEmptyWhenCodeNotFound()
    {
        //Arrange
        var notValidCode = _fixture.Create<string>();

        //Act
        var message = FhirHttpErrorHelper.GetDisplayMessageByCode(notValidCode);

        //Assert
        message.Should().BeEmpty();
    }
}
