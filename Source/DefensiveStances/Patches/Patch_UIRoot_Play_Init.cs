using DefensiveStances.Utilities;
using HarmonyLib;
using RimWorld;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(UIRoot_Play), nameof(UIRoot_Play.Init))]
    internal static class Patch_UIRoot_Play_Init
    {
        private static void Postfix(UIRoot_Play __instance)
        {
            DefensiveAlertRegistrar.EnsureRegistered(__instance.alerts);
        }
    }
}
