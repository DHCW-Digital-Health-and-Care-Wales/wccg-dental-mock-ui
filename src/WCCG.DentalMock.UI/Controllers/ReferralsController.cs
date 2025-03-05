using Microsoft.AspNetCore.Mvc;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Extensions;
using WCCG.DentalMock.UI.Services;
using WCCG.DentalMock.UI.Swagger;

namespace WCCG.DentalMock.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferralsController : ControllerBase
{
    private readonly IReferralService _referralService;
    private readonly ILogger<ReferralsController> _logger;

    public ReferralsController(IReferralService referralService, ILogger<ReferralsController> logger)
    {
        _referralService = referralService;
        _logger = logger;
    }

    [HttpPost("createReferral")]
    [SwaggerProcessMessageRequest]
    public async Task<IActionResult> CreateReferral()
    {
        _logger.CalledMethod(nameof(CreateReferral));

        using var reader = new StreamReader(HttpContext.Request.Body);
        var bundleJson = await reader.ReadToEndAsync();

        var response = await _referralService.CreateReferralAsync(bundleJson, HttpContext.Request.Headers);

        return new ContentResult
        {
            StatusCode = (int)response.StatusCode,
            Content = await response.Content.ReadAsStringAsync(),
            ContentType = FhirConstants.FhirMediaType
        };
    }
}
