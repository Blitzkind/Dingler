extern alias HexGame;
using HarmonyLib;
using System.Reflection;

namespace Dingler.Game.HarmonyPatches
{
                    public static class HarmonyPatcher
    {
        private static readonly Harmony _harmony = new Harmony("Patch");
        public static void Patch()
        {
            try
            {
                _harmony.PatchAll(Assembly.GetExecutingAssembly());
                StaticLogger.LogInformation("Harmony patching complete. Patched methods: {count}",
                    _harmony.GetPatchedMethods().Count());
            }
            catch (Exception ex)
            {
                StaticLogger.LogError(ex, "Error patching harmony.");
            }
        }
    }
}
