using System.Net;
using System.Net.Mime;
using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Middleware;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;
using Task = System.Threading.Tasks.Task;

namespace WCCG.DentalMock.UI.Integration.Tests.Middleware;

public class ResponseMiddlewareTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();

    [Fact]
    public async Task ShouldHandleExceptions()
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

        var operationOutcome = JsonSerializer.Deserialize<ProblemDetails>(await response.Content.ReadAsStringAsync())!;
        operationOutcome.Title.Should().Be("Unexpected error");
        operationOutcome.Detail.Should().Be(exception.Message);
    }

    [Fact]
    public async Task ShouldTryToAddHeadersWhenException()
    {
        //Arrange
        var exception = _fixture.Create<Exception>();
        var requestId = _fixture.Create<string>();
        var correlationId = _fixture.Create<string>();

        var host = StartHostWithException(exception);

        //Act
        var response = await host.GetTestServer()
            .CreateRequest(HostProvider.TestEndpoint)
            .AddHeader(RequestHeaderKeys.RequestId, requestId)
            .AddHeader(RequestHeaderKeys.CorrelationId, correlationId)
            .GetAsync();

        //Assert
        response.Headers.GetValues("X-Operation-Id").Should()
            .NotBeNullOrEmpty();

        response.Content.Headers.GetValues(HeaderNames.ContentType).Should()
            .NotBeNull()
            .And.Contain(MediaTypeNames.Application.Json);

        response.Headers.GetValues(RequestHeaderKeys.RequestId).Should()
            .NotBeNull()
            .And.Contain(requestId);

        response.Headers.GetValues(RequestHeaderKeys.CorrelationId).Should()
            .NotBeNull()
            .And.Contain(correlationId);
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
            configureApp: app => app.UseMiddleware<ResponseMiddleware>());
    }

    private static IHost StartHost()
    {
        return HostProvider.StartHostWithEndpoint(_ => Task.CompletedTask,
            configureApp: app => app.UseMiddleware<ResponseMiddleware>());
    }
}
