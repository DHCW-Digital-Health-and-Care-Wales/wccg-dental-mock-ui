using System.Net;
using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Errors;
using WCCG.DentalMock.UI.Exceptions;
using WCCG.DentalMock.UI.Extensions;
using WCCG.DentalMock.UI.Helpers;
using WCCG.DentalMock.UI.Middleware;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;
using Task = System.Threading.Tasks.Task;

namespace WCCG.DentalMock.UI.Integration.Tests.Middleware;

public class ResponseMiddlewareTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();

    [Fact]
    public async Task ShouldHandleReferralCreationException()
    {
        //Arrange
        var exception = new ReferralCreationException(_fixture.Create<HttpStatusCode>(), OperationOutcomeCreator.CreateOperationOutcome(
            OperationOutcome.IssueType.Transient, new UnexpectedExternalError(_fixture.Create<Exception>())));

        var host = StartHostWithException(exception);

        //Act
        var response = await host.GetTestServer()
            .CreateRequest(HostProvider.TestEndpoint)
            .GetAsync();

        //Assert
        response.StatusCode.Should().Be(exception.StatusCode);

        var operationOutcome = JsonSerializer.Deserialize<OperationOutcome>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions().ForFhirExtended())!;
        operationOutcome.Should().BeEquivalentTo(exception.OperationOutcome);
    }

    [Fact]
    public async Task ShouldHandleHttpRequestException()
    {
        //Arrange
        var exception = _fixture.Create<HttpRequestException>();

        var host = StartHostWithException(exception);

        //Act
        var response = await host.GetTestServer()
            .CreateRequest(HostProvider.TestEndpoint)
            .GetAsync();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

        var operationOutcome = JsonSerializer.Deserialize<OperationOutcome>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions().ForFhirExtended())!;
        operationOutcome.Issue.Should().AllSatisfy(component =>
        {
            component.Code.Should().Be(OperationOutcome.IssueType.Transient);
            component.Severity.Should().Be(OperationOutcome.IssueSeverity.Error);
            component.Diagnostics.Should().Contain(exception.Message);
        });
    }

    [Fact]
    public async Task ShouldHandleDefaultException()
    {
        //Arrange
        var exception = _fixture.Create<Exception>();

        var host = StartHostWithException(exception);

        //Act
        var response = await host.GetTestServer()
            .CreateRequest(HostProvider.TestEndpoint)
            .GetAsync();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var operationOutcome = JsonSerializer.Deserialize<OperationOutcome>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions().ForFhirExtended())!;
        operationOutcome.Issue.Should().AllSatisfy(component =>
        {
            component.Code.Should().Be(OperationOutcome.IssueType.Transient);
            component.Severity.Should().Be(OperationOutcome.IssueSeverity.Error);
            component.Diagnostics.Should().Contain(exception.Message);
        });
    }

    [Fact]
    public async Task ShouldTryToAddHeadersWhenException()
    {
        //Arrange
        var exception = _fixture.Create<Exception>();
        var host = StartHostWithException(exception);

        //Act
        var response = await host.GetTestServer()
            .CreateRequest(HostProvider.TestEndpoint)
            .GetAsync();

        //Assert
        response.Headers.GetValues("X-Operation-Id").Should()
            .NotBeNullOrEmpty();

        response.Content.Headers.GetValues(HeaderNames.ContentType).Should()
            .NotBeNull()
            .And.Contain(FhirConstants.FhirMediaType);
    }

    [Fact]
    public async Task ShouldTryToAddHeadersWhenSuccessful()
    {
        //Arrange
        var requestId = _fixture.Create<string>();
        var correlationId = _fixture.Create<string>();

        var host = StartHost();

        //Act
        var response = await host.GetTestServer()
            .CreateRequest(HostProvider.TestEndpoint)
            .AddHeader(RequestHeaderKeys.RequestId, requestId)
            .AddHeader(RequestHeaderKeys.CorrelationId, correlationId)
            .GetAsync();

        //Assert
        response.Headers.GetValues("X-Operation-Id").Should()
            .NotBeNullOrEmpty();

        response.Headers.GetValues(RequestHeaderKeys.RequestId).Should()
            .NotBeNull()
            .And.Contain(requestId);

        response.Headers.GetValues(RequestHeaderKeys.CorrelationId).Should()
            .NotBeNull()
            .And.Contain(correlationId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private static IHost StartHostWithException(Exception exception)
    {
        return HostProvider.StartHostWithEndpoint(_ => throw exception,
            addServices: services => services.AddSingleton(new JsonSerializerOptions().ForFhirExtended()),
            configureApp: app => app.UseMiddleware<ResponseMiddleware>());
    }

    private static IHost StartHost()
    {
        return HostProvider.StartHostWithEndpoint(_ => Task.CompletedTask,
            addServices: services => services.AddSingleton(new JsonSerializerOptions().ForFhirExtended()),
            configureApp: app => app.UseMiddleware<ResponseMiddleware>());
    }
}
