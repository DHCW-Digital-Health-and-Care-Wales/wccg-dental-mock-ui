using System.Diagnostics.CodeAnalysis;

namespace WCCG.DentalMock.UI.Configuration;

[ExcludeFromCodeCoverage]
public class ApplicationInsightsConfig
{
    public static string SectionName => "ApplicationInsights";

    public required string ConnectionString { get; set; }
}
