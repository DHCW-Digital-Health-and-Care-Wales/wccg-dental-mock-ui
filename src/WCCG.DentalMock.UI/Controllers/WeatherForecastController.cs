using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using WCCG.DentalMockUI.Web.Models;
using LoggerExtensions = WCCG.DentalMock.UI.Extensions.LoggerExtensions;

namespace WCCG.DentalMock.UI.Controllers;

[ApiController]
[Route("[controller]")]
[ExcludeFromCodeCoverage]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        LoggerExtensions.CalledMethod(_logger, nameof(Get));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
