using System.Diagnostics.CodeAnalysis;
using WCCG.DentalMock.UI.Exceptions;

namespace WCCG.DentalMock.UI.Extensions;

[ExcludeFromCodeCoverage]
public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "Called {methodName}.")]
    public static partial void CalledMethod(this ILogger logger, string methodName);

    [LoggerMessage(Level = LogLevel.Error, Message = "API call unexpected error.")]
    public static partial void ApiCallError(this ILogger logger, HttpRequestException exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to create referral. Reason")]
    public static partial void ReferralCreationError(this ILogger logger, ReferralCreationException exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "Unexpected error.")]
    public static partial void UnexpectedError(this ILogger logger, Exception exception);
}
