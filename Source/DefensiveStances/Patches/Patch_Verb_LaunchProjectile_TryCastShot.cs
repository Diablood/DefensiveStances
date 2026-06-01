using DefensiveStances.Utilities;
using HarmonyLib;
using Verse;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(Verb_LaunchProjectile), "TryCastShot")]
    internal static class Patch_Verb_LaunchProjectile_TryCastShot
    {
        private static void Postfix(Verb_LaunchProjectile __instance, bool __result)
        {
            if (!__result)
            {
                return;
            }

            DefensiveAggressionUtility.RecordDirectAttack(__instance.Caster, __instance.CurrentTarget.Pawn);
        }
    }
}
