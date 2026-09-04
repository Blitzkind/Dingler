extern alias HexGame;
using Dingler.Game.GameObjects;
using HarmonyLib;
using HexGame::Game.Shared;

namespace Dingler.Game.HarmonyPatches
{
    public static class TimerPatches
    {
        [HarmonyPatch(typeof(Player), "IsInactiveTimerExpired")]
        public static class IsInactiveTimerExpiredPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance, ref bool __result)
            {
                if (__instance is not TrackedPlayer player)
                    return true;

                var timer = player.GameTimer;
                __result = timer.HasTimeExpired;
                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "IsChessTimerExpired")]
        public static class IsChessTimerExpiredPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance, ref bool __result)
            {
                if (!__instance.m_Session.TimersEnabled ||__instance is not TrackedPlayer player)
                    return true;

                var timer = player.GameTimer;
                __result = timer.HasTimeExpired;
                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "StartChessTimer")]
        public static class StartTimerPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance)
            {
                if (__instance is not TrackedPlayer player)
                    return true;

                var timer = player.GameTimer;
                
                timer.StartChessTimer();
                return false;
            }
        }
        

        [HarmonyPatch(typeof(Player), "StopChessTimer")]
        public static class StopTimerPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance)
            {
                if (__instance is not TrackedPlayer player)
                    return true;

                var timer = player.GameTimer;
                
                timer.StopChessTimer();
                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "GrantFudgeTime")]
        public static class GrantFudgeTimePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance)
            {
                if (__instance is not TrackedPlayer player)
                    return true;
                
                player.GameTimer.AddFudgeTime();
                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "ResetInactivityTimer")]
        public static class ResetInactivityTimerPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance)
            {
                if (__instance is not TrackedPlayer player)
                    return true;

                var timer = player.GameTimer;
                
                timer.StopChessTimer();
                timer.StartChessTimer();

                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "ResumeChessTimer")]
        public static class ResumeChessTimerPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance)
            {
                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "PauseChessTimer")]
        public static class PauseChessTimerPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance)
            {
                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "GetChessTimerElapsedTime")]
        public static class GetChessTimerElapsedTimePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance, ref TimeSpan __result)
            {
                if (__instance is not TrackedPlayer player)
                    return true;

                __result = player.GameTimer.ElapsedTime;
                return false;
            }
        }
        
        [HarmonyPatch(typeof(Player), "SetChessTimerElapsedTime")]
        public static class SetChessTimerElapsedTimePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ref Player __instance, TimeSpan time)
            {
                if (__instance is not TrackedPlayer player)
                    return true;

                var timer = player.GameTimer;
                timer.ElapsedTime = time;
                return false;
            }
        }
    }
}
