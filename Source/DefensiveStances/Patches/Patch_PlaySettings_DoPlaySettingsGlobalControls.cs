using DefensiveStances.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DefensiveStances.Patches
{
    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    internal static class Patch_PlaySettings_DoPlaySettingsGlobalControls
    {
        private static void Postfix(WidgetRow row, bool worldView)
        {
            if (!worldView)
            {
                DefensiveSafeAreaOverlay.DrawToggle(row);
                DefensiveGlobalEmergencyEvacuationUI.DrawToggle(row);
            }
        }
    }
}
