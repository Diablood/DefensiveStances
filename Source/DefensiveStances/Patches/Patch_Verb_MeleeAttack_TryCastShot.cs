using DefensiveStances.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(Verb_MeleeAttack), "TryCastShot")]
    internal static class Patch_Verb_MeleeAttack_TryCastShot
    {
        private static void Prefix(Verb_MeleeAttack __instance)
        {
            Pawn aggressor = __instance.CasterPawn;
            Pawn defender = __instance.CurrentTarget.Pawn;
            if (aggressor == null || defender == null || !aggressor.Spawned || aggressor.stances == null || aggressor.stances.FullBodyBusy)
            {
                return;
            }

            DefensiveAggressionUtility.RecordDirectAttack(aggressor, defender);
        }
    }
}
