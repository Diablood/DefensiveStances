using HarmonyLib;
using Verse;

namespace DefensiveStances
{
    [StaticConstructorOnStartup]
    internal static class DefensiveStancesBootstrap
    {
        internal const string HarmonyId = "diablood.defensivestances";

        static DefensiveStancesBootstrap()
        {
            new Harmony(HarmonyId).PatchAll();
            DS_Log.Message($"Version {typeof(DefensiveStancesBootstrap).Assembly.GetName().Version} loaded. Harmony patches applied.");
        }
    }
}
