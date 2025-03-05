using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WCCG.DentalMock.UI.Configuration;

[ExcludeFromCodeCoverage]
public class EReferralsApiConfig
{
    public static string SectionName => "EReferralsApi";

    [Required]
    public required string ApimEndpoint { get; set; }

    [Required]
    public required string ApimSubscriptionHeaderName { get; init; }

    [Required]
    public required string ApimSubscriptionKey { get; init; }

    [Required]
    public required string CreateReferralEndpoint { get; set; }

    [Required]
    public required int TimeoutSeconds { get; set; }
}
