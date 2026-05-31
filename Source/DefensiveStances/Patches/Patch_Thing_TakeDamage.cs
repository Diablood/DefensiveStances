using DefensiveStances.Components;
using DefensiveStances.Domain;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(Thing), nameof(Thing.TakeDamage))]
    internal static class Patch_Thing_TakeDamage
    {
        private static void Prefix(Thing __instance, DamageInfo dinfo)
        {
            Pawn pawn = __instance as Pawn;
            Thing aggressor = dinfo.Instigator;
            if (pawn == null || aggressor == null || aggressor == pawn || !GenHostility.HostileTo(aggressor, pawn))
            {
                return;
            }

            DefensivePawnState state = DefensiveStancesGameComponent.Current?.GetPawnState(pawn, false);
            if (state?.mode == DefensiveBehaviorMode.SelfDefenseOnly)
            {
                state.RecordAggression(aggressor);
            }
        }
    }
}
