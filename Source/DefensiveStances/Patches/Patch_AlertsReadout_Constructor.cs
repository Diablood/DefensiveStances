using DefensiveStances.Utilities;
using HarmonyLib;
using RimWorld;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(AlertsReadout), MethodType.Constructor)]
    internal static class Patch_AlertsReadout_Constructor
    {
        private static void Postfix(AlertsReadout __instance)
        {
            DefensiveAlertRegistrar.EnsureRegistered(__instance);
        }
    }
}
