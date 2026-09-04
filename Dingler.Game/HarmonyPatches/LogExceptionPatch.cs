using System.Text;
using HarmonyLib;

namespace Dingler.Game.HarmonyPatches;

extern alias HexGame;

[HarmonyPatch(typeof(HexGame::LogBase), "Exception", [typeof(string), typeof(HexGame.ELogLevel), typeof(Exception), typeof(string), typeof(object[])])]
public static class LogExceptionPatch
{
	[HarmonyPrefix]
	public static bool Prefix(string loggerName, Exception ex, string format, object[] args)
	{
		if (ex is ArgumentOutOfRangeException && loggerName.ToUpper().Equals("ENCDATA"))
			return false;

		if (format.Equals("Error! Error!"))
		{
			format = "Exception occured!";
		}

		StringBuilder builder = new StringBuilder().Append(format);

		builder.Append($"\n-> Exception type is {ex.GetType().FullName}");

		if (ex.InnerException is not null)
		{
			builder.Append($"\n-> Inner exception type is {ex.InnerException.GetType().FullName}\n-> Inner exception message: {ex.InnerException.Message}\n-> Inner stack trace:\n{ex.InnerException.StackTrace}");
		}

		builder.Append($"\n-> Exception message: {ex.Message}\n-> Stack trace:\n{ex.StackTrace}");

		StaticLogger.LogError(builder.ToString());

		return false;
	}
}