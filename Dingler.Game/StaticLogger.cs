using Microsoft.Extensions.Logging;

// To only be used in static classes

namespace Dingler.Game;

public static class StaticLogger
{
	private static ILogger? _logger;

	public static void LogDebug(string message, params object[] args) => _logger?.LogDebug(message, args); 
	public static void LogWarning(string message, params object[] args) => _logger?.LogWarning(message, args); 
	public static void LogTrace(string message, params object[] args) => _logger?.LogTrace(message, args); 
	public static void LogInformation(string message, params object[] args) => _logger?.LogInformation(message, args); 
	public static void LogError(string message, params object[] args) => _logger?.LogError(message, args); 
	public static void LogError(Exception ex, string message) => _logger?.LogError(ex, message); 
	public static void LogCritical(string message, params object[] args) => _logger?.LogCritical(message, args); 
	public static void SetLogger(ILogger logger) => _logger = logger;
}

