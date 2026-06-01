using System.Reflection;
using DefensiveStances.Components;
using DefensiveStances.Domain;
using DefensiveStances.Settings;
using DefensiveStances.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace DefensiveStances.Patches
{
    [HarmonyPatch]
    internal static class Patch_JobGiver_ConfigurableHostilityResponse_TryGiveJob
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(JobGiver_ConfigurableHostilityResponse), "TryGiveJob");
        }

        private static bool Prefix(Pawn pawn, ref Job __result)
        {
            if (!DefensiveBehaviorUtility.CanConfigure(pawn) || PawnUtility.PlayerForcedJobNowOrSoon(pawn) || pawn.Downed)
            {
                return true;
            }

            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            DefensivePawnState state = component?.GetPawnState(pawn, false);
            if (state == null || state.mode == DefensiveBehaviorMode.Vanilla)
            {
                return true;
            }

            if (state.mode == DefensiveBehaviorMode.SelfDefenseOnly)
            {
                __result = DefensiveResponseJobFactory.TryCreateSelfDefenseJob(pawn, state);
                return false;
            }

            if (!SelfDefenseUtility.ShouldStartFleeing(pawn))
            {
                __result = null;
                return false;
            }

            Area safeArea = component.GetSafeArea(pawn.Map);
            Job evacuationJob;
            if (DefensiveEvacuationUtility.TryCreateEvacuationJob(pawn, state, safeArea, out evacuationJob))
            {
                __result = evacuationJob;
                return false;
            }

            if (DefensiveStancesSettings.Current.allowVanillaFleeFallback)
            {
                // No valid configured area: preserve vanilla flee behavior as an optional safe fallback.
                return true;
            }

            __result = null;
            return false;
        }
    }
}
