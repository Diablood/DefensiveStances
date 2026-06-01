using DefensiveStances.Components;
using HarmonyLib;
using RimWorld;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(Pawn_DraftController), nameof(Pawn_DraftController.Drafted), MethodType.Setter)]
    internal static class Patch_Pawn_DraftController_Drafted
    {
        private static void Postfix(Pawn_DraftController __instance)
        {
            if (!__instance.Drafted)
            {
                DefensiveStancesGameComponent.Current?.NotifyPawnUndrafted(__instance.pawn);
            }
        }
    }
}
