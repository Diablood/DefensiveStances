using DefensiveStances.UI;
using DefensiveStances.Utilities;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(HostilityResponseModeUtility), nameof(HostilityResponseModeUtility.DrawResponseButton))]
    internal static class Patch_HostilityResponseModeUtility_DrawResponseButton
    {
        private static bool Prefix(Rect rect, Pawn pawn, bool paintable)
        {
            if (!DefensiveBehaviorUtility.CanConfigure(pawn))
            {
                return true;
            }

            DefensiveHostilityResponseUI.DrawResponseButton(rect, pawn, paintable);
            return false;
        }
    }
}
