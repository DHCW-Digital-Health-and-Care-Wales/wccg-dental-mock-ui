﻿using System.Diagnostics.CodeAnalysis;

namespace WCCG.DentalMockUI.Web.Extensions;

[ExcludeFromCodeCoverage]
public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Called {methodName}.")]
    public static partial void CalledMethod(this ILogger logger, string methodName);
}
