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
            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            if (__instance.Drafted)
            {
                component?.GetPawnState(__instance.pawn, false)?.ClearAggression();
                return;
            }

            component?.NotifyPawnUndrafted(__instance.pawn);
        }
    }
}
