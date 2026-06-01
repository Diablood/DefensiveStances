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
            if (!visible)
            {
                return;
            }

            DefensiveSafeAreaUtility.Get(Find.CurrentMap)?.MarkForDraw();
        }
    }
}
