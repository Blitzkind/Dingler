extern alias HexGame;
using HarmonyLib;

namespace Dingler.Game.HarmonyPatches
{
    // Hex's logging is set to log to the console (or output.log, but I'm not using that). This intercepts the logging calls and forwards them to whatever logging library we decide to use.
    [HarmonyPatch(typeof(HexGame.LogBase), "Write", [
        typeof(string), typeof(HexGame.ELogLevel), typeof(string), typeof(object[]) ])]
    public static class LogWritePatch
    {
        [HarmonyPrefix]
        public static bool Prefix(string loggerName, HexGame.ELogLevel level, string text, object[] args)
        {
            switch (level)
            {
                case HexGame.ELogLevel.Debug:
                    StaticLogger.LogDebug(text, args);
                    break;
                case HexGame.ELogLevel.Warning:
                    StaticLogger.LogWarning(text, args);
                    break;
                case HexGame.ELogLevel.Trace:
                    StaticLogger.LogTrace(text, args);
                    break;
                case HexGame.ELogLevel.Info:
                    StaticLogger.LogInformation(text, args);
                    break;
                case HexGame.ELogLevel.Error:
                    StaticLogger.LogError(text, args);
                    break;
                case HexGame.ELogLevel.Fatal:
                    StaticLogger.LogCritical(text, args);
                    break;
                case HexGame.ELogLevel.Success:
                    StaticLogger.LogInformation(text, args);
                    break;
            }
            return false;
        }
    }
}
