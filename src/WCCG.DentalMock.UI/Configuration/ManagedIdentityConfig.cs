using System.Diagnostics.CodeAnalysis;

namespace WCCG.DentalMock.UI.Configuration;

[ExcludeFromCodeCoverage]
public class ManagedIdentityConfig
{
    public static string SectionName => "ManagedIdentity";

    public required string ClientId { get; set; }
}
