using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Core.Extensions;

public static class LoggerExtensions
{
    public static void Log(this ILogger logger, LogLevel logLevel, IDictionary<string, object> dictionary)
    {
        (string msgTemplate, object[] parameters) = GenerateMessageTemplate(dictionary);
        logger.Log(logLevel, msgTemplate, parameters);
    }

    public static void LogInformation(this ILogger logger, IDictionary<string, object> dictionary) =>
        logger.Log(LogLevel.Information, dictionary);

    public static void LogWarning(this ILogger logger, IDictionary<string, object> dictionary) =>
        logger.Log(LogLevel.Warning, dictionary);

    public static void LogError(this ILogger logger, IDictionary<string, object> dictionary) =>
        logger.Log(LogLevel.Error, dictionary);

    public static void LogCritical(this ILogger logger, IDictionary<string, object> dictionary) =>
        logger.Log(LogLevel.Critical, dictionary);

    public static void LogNone(this ILogger logger, IDictionary<string, object> dictionary) =>
        logger.Log(LogLevel.None, dictionary);

    public static void LogDebug(this ILogger logger, IDictionary<string, object> dictionary) =>
        logger.Log(LogLevel.Debug, dictionary);

    public static void LogTrace(this ILogger logger, IDictionary<string, object> dictionary) =>
        logger.Log(LogLevel.Trace, dictionary);

    private static (string, object[]) GenerateMessageTemplate(IDictionary<string, object> dictionary)
    {
        var messageTemplate = new StringBuilder();
        var parameters = new List<object>();

        foreach (var kvp in dictionary)
        {
            messageTemplate.Append($"{kvp.Key} = {{{kvp.Key}}}, ");
            parameters.Add(kvp.Value);
        }

        // Remove the last comma and space
        if (messageTemplate.Length >= 2)
        {
            messageTemplate.Length -= 2;
        }

        return (messageTemplate.ToString(), parameters.ToArray());
    }
     

}
