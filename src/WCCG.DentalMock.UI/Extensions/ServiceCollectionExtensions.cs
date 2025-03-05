using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using WCCG.DentalMock.UI.Configuration;
using WCCG.DentalMock.UI.Services;

namespace WCCG.DentalMock.UI.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddApplicationInsights(this IServiceCollection services, bool isDevelopmentEnvironment, IConfiguration configuration)
    {
        var appInsightsConnectionString = configuration.GetRequiredSection(ApplicationInsightsConfig.SectionName)
            .GetValue<string>(nameof(ApplicationInsightsConfig.ConnectionString));

        services.AddApplicationInsightsTelemetry(options => options.ConnectionString = appInsightsConnectionString);
        services.Configure<TelemetryConfiguration>(config =>
        {
            if (isDevelopmentEnvironment)
            {
                config.SetAzureTokenCredential(new AzureCliCredential());
                return;
            }

            var clientId = configuration.GetRequiredSection(ManagedIdentityConfig.SectionName)
                .GetValue<string>(nameof(ManagedIdentityConfig.ClientId));
            config.SetAzureTokenCredential(new ManagedIdentityCredential(clientId));
        });
    }

    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IReferralService, ReferralService>((provider, client) =>
        {
            var eReferralsApiConfig = provider.GetRequiredService<IOptions<EReferralsApiConfig>>().Value;
            client.BaseAddress = new Uri(eReferralsApiConfig.ApimEndpoint);
            client.DefaultRequestHeaders.Add(eReferralsApiConfig.ApimSubscriptionHeaderName, eReferralsApiConfig.ApimSubscriptionKey);
            client.Timeout = TimeSpan.FromSeconds(eReferralsApiConfig.TimeoutSeconds);
        });
    }
}
