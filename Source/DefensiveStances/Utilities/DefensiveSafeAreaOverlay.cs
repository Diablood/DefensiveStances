using DefensiveStances.Components;
using RimWorld;
using Verse;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveSafeAreaOverlay
    {
        private static bool visible;

        internal static void DrawToggle(WidgetRow row)
        {
            row.ToggleableIcon(
                ref visible,
                TexButton.ShowZones,
                "DS_SafeAreaVisibilityToggleButton".Translate(),
                SoundDefOf.Mouseover_ButtonToggle);
        }

        internal static void DrawIfVisible()
        {
            Map map = Find.CurrentMap;
            if (!visible && DefensiveStancesGameComponent.Current?.IsGlobalEmergencyEvacuationActive(map) != true)
            {
                return;
            }

            DefensiveSafeAreaUtility.Get(map)?.MarkForDraw();
        }
    }
}
