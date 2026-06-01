using DefensiveStances.Utilities;
using HarmonyLib;
using RimWorld;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(MapInterface), nameof(MapInterface.MapInterfaceUpdate))]
    internal static class Patch_MapInterface_MapInterfaceUpdate
    {
        private static void Postfix()
        {
            DefensiveSafeAreaOverlay.DrawIfVisible();
        }
    }
}
