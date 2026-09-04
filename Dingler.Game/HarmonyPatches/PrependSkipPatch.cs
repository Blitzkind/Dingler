extern alias HexGame;
using System.Reflection;
using HarmonyLib;
using HexGame::Game.Shared;

namespace Dingler.Game.HarmonyPatches;

extern alias HexGame;

[HarmonyPatch]
public static class PrependSkipPatch
{
	static MethodInfo TargetMethod()
	{
		var logType = typeof(HexGame::Log);

		var paramTypes = new Type[]
		{
			typeof(string),
			typeof(UID),
			typeof(object[]).MakeByRefType()
		};

		var method = AccessTools.Method(logType, "PrependUidToLogMessage", paramTypes);

		return method == null
			? throw new Exception("PrependSkipPatch: couldn't find PrependUidToLogMessage. " +
			                      "Check that HexGame.Log is the expected type and that the signature is (string, UID, ref object[]).")
			: method;
	}

	[HarmonyPrefix]
	public static bool Prefix(string text, UID uid, ref object[] args, ref string __result)
	{
		__result = text;
		return false;
	}
}