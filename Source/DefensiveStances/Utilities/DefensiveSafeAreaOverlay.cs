using DefensiveStances.Components;
using RimWorld;
using RimWorld.Planet;
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
            if (!WorldRendererUtility.DrawingMap)
            {
                return;
            }

            Map map = Find.CurrentMap;
            if (map == null)
            {
                return;
            }

            if (!visible && DefensiveStancesGameComponent.Current?.IsGlobalEmergencyEvacuationActive(map) != true)
            {
                return;
            }

            DefensiveSafeAreaUtility.Get(map)?.MarkForDraw();
        }
    }
}
