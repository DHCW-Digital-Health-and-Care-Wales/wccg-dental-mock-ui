using System.Globalization;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using RichardSzalay.MockHttp;
using WCCG.DentalMock.UI.Configuration;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Services;
using WCCG.DentalMock.UI.Unit.Tests.Extensions;
using Task = System.Threading.Tasks.Task;

namespace WCCG.DentalMock.UI.Unit.Tests.Services;

public class ReferralServiceTests
{
    private readonly IFixture _fixture = new Fixture().WithCustomizations();
    private readonly EReferralsApiConfig _eReferralsApiConfig;

    public ReferralServiceTests()
    {
        _eReferralsApiConfig = _fixture.Create<EReferralsApiConfig>();
        _fixture.Mock<IOptions<EReferralsApiConfig>>().SetupGet(x => x.Value).Returns(_eReferralsApiConfig);
    }

    [Fact]
    public async Task CreateReferralAsyncShouldAddHeaders()
    {
        //Arrange
        var headers = new HeaderDictionary();
        foreach (var header in RequestHeaderKeys.GetAll())
        {
            if (header == RequestHeaderKeys.Accept)
            {
                headers.Add(header, new StringValues("application/fhir+json; version=1.0"));
                continue;
            }

            headers.Add(header, _fixture.Create<string>());
        }

        var bundleJson = _fixture.Create<string>();
        var expectedResponse = _fixture.Create<string>();

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"/{_eReferralsApiConfig.CreateReferralEndpoint}")
            .WithContent(bundleJson)
            .WithHeaders(HeaderNames.ContentType, FhirConstants.FhirMediaType)
            .WithHeaders(RequestHeaderKeys.Accept, headers[RequestHeaderKeys.Accept]!)
            .WithHeaders(RequestHeaderKeys.CorrelationId, headers[RequestHeaderKeys.CorrelationId]!)
            .WithHeaders(RequestHeaderKeys.EndUserOrganisation, headers[RequestHeaderKeys.EndUserOrganisation]!)
            .WithHeaders(RequestHeaderKeys.RequestId, headers[RequestHeaderKeys.RequestId]!)
            .WithHeaders(RequestHeaderKeys.RequestingSoftware, headers[RequestHeaderKeys.RequestingSoftware]!)
            .WithHeaders(RequestHeaderKeys.TargetIdentifier, headers[RequestHeaderKeys.TargetIdentifier]!)
            .WithHeaders(RequestHeaderKeys.UseContext, headers[RequestHeaderKeys.UseContext]!)
            .Respond(FhirConstants.FhirMediaType, expectedResponse);

        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://some.com");

        var sut = new ReferralService(httpClient,
            _fixture.Mock<IOptions<EReferralsApiConfig>>().Object);

        //Act
        await sut.CreateReferralAsync(bundleJson, headers);

        //Assert
        mockHttp.VerifyNoOutstandingExpectation();
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Fact]
    public async Task CreateReferralAsyncShouldReturnResponse()
    {
        //Arrange
        var bundleJson = _fixture.Create<string>();
        var expectedResponse = _fixture.Create<HttpResponseMessage>();

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"/{_eReferralsApiConfig.CreateReferralEndpoint}")
            .WithContent(bundleJson)
            .Respond(_ => expectedResponse);

        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://some.com");

        var sut = new ReferralService(httpClient, _fixture.Mock<IOptions<EReferralsApiConfig>>().Object);

        //Act
        var result = await sut.CreateReferralAsync(bundleJson, _fixture.Create<IHeaderDictionary>());

        //Assert
        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task GetReferralAsyncShouldAddHeaders()
    {
        //Arrange
        var headers = new HeaderDictionary();
        foreach (var header in RequestHeaderKeys.GetAll())
        {
            if (header == RequestHeaderKeys.Accept)
            {
                headers.Add(header, new StringValues("application/fhir+json; version=1.0"));
                continue;
            }

            headers.Add(header, _fixture.Create<string>());
        }

        var id = _fixture.Create<string>();
        var expectedResponse = _fixture.Create<string>();

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, string.Format(CultureInfo.InvariantCulture, $"/{_eReferralsApiConfig.GetReferralEndpoint}", id))
            .WithHeaders(RequestHeaderKeys.Accept, headers[RequestHeaderKeys.Accept]!)
            .WithHeaders(RequestHeaderKeys.CorrelationId, headers[RequestHeaderKeys.CorrelationId]!)
            .WithHeaders(RequestHeaderKeys.EndUserOrganisation, headers[RequestHeaderKeys.EndUserOrganisation]!)
            .WithHeaders(RequestHeaderKeys.RequestId, headers[RequestHeaderKeys.RequestId]!)
            .WithHeaders(RequestHeaderKeys.RequestingSoftware, headers[RequestHeaderKeys.RequestingSoftware]!)
            .WithHeaders(RequestHeaderKeys.TargetIdentifier, headers[RequestHeaderKeys.TargetIdentifier]!)
            .WithHeaders(RequestHeaderKeys.UseContext, headers[RequestHeaderKeys.UseContext]!)
            .Respond(FhirConstants.FhirMediaType, expectedResponse);

        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://some.com");

        var sut = new ReferralService(httpClient,
            _fixture.Mock<IOptions<EReferralsApiConfig>>().Object);

        //Act
        await sut.GetReferralAsync(id, headers);

        //Assert
        mockHttp.VerifyNoOutstandingExpectation();
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Fact]
    public async Task GetReferralAsyncShouldReturnResponse()
    {
        //Arrange
        var id = _fixture.Create<string>();
        var expectedResponse = _fixture.Create<HttpResponseMessage>();

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, string.Format(CultureInfo.InvariantCulture, $"/{_eReferralsApiConfig.GetReferralEndpoint}", id))
            .Respond(_ => expectedResponse);

        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://some.com");

        var sut = new ReferralService(httpClient, _fixture.Mock<IOptions<EReferralsApiConfig>>().Object);

        //Act
        var result = await sut.GetReferralAsync(id, _fixture.Create<IHeaderDictionary>());

        //Assert
        result.Should().Be(expectedResponse);
    }
}
