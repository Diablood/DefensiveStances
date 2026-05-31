using System.Collections.Generic;
using System.Reflection;
using DefensiveStances.UI;
using DefensiveStances.Utilities;
using HarmonyLib;
using Verse;

namespace DefensiveStances.Patches
{
    [HarmonyPatch]
    internal static class Patch_Pawn_GetGizmos
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Pawn), nameof(Thing.GetGizmos));
        }

        private static void Postfix(Thing __instance, ref IEnumerable<Gizmo> __result)
        {
            Pawn pawn = __instance as Pawn;
            if (!DefensiveBehaviorUtility.CanConfigure(pawn))
            {
                return;
            }

            __result = Append(__result, pawn);
        }

        private static IEnumerable<Gizmo> Append(IEnumerable<Gizmo> original, Pawn pawn)
        {
            if (original != null)
            {
                foreach (Gizmo gizmo in original)
                {
                    yield return gizmo;
                }
            }

            foreach (Gizmo gizmo in DefensiveStancesGizmoFactory.CreateFor(pawn))
            {
                yield return gizmo;
            }
        }
    }
}
