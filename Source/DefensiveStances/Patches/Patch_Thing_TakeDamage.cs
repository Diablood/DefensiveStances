using DefensiveStances.Utilities;
using HarmonyLib;
using Verse;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(Thing), nameof(Thing.TakeDamage))]
    internal static class Patch_Thing_TakeDamage
    {
        private static void Prefix(Thing __instance, DamageInfo dinfo)
        {
            DefensiveAggressionUtility.RecordDirectAttack(dinfo.Instigator, __instance as Pawn);
        }
    }
}
