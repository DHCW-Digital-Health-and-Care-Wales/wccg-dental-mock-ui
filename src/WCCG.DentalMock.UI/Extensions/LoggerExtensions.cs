using System.Diagnostics.CodeAnalysis;

namespace WCCG.DentalMock.UI.Extensions;

[ExcludeFromCodeCoverage]
public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "Called {methodName}.")]
    public static partial void CalledMethod(this ILogger logger, string methodName);

    [LoggerMessage(Level = LogLevel.Error, Message = "Unexpected error.")]
    public static partial void UnexpectedError(this ILogger logger, Exception exception);
}
