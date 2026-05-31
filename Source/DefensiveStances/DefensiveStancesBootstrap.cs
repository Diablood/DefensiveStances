using HarmonyLib;
using Verse;

namespace DefensiveStances
{
    [StaticConstructorOnStartup]
    internal static class DefensiveStancesBootstrap
    {
        internal const string HarmonyId = "todoauthor.defensivestances";

        static DefensiveStancesBootstrap()
        {
            new Harmony(HarmonyId).PatchAll();
            Log.Message("[Defensive Stances] Harmony patches applied.");
        }
    }
}
