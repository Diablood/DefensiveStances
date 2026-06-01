using DefensiveStances.Components;
using RimWorld;
using UnityEngine;
using Verse;

namespace DefensiveStances.Utilities
{
    [StaticConstructorOnStartup]
    internal static class DefensiveGlobalEmergencyEvacuationUI
    {
        private static readonly Texture2D AlarmIcon = ContentFinder<Texture2D>.Get("UI/Buttons/GlobalEmergencyEvacuation");

        internal static void DrawToggle(WidgetRow row)
        {
            Map map = Find.CurrentMap;
            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            if (map == null || component == null)
            {
                return;
            }

            bool active = component.IsGlobalEmergencyEvacuationActive(map);
            bool requestedState = active;
            row.ToggleableIcon(
                ref requestedState,
                AlarmIcon,
                active
                    ? "DS_GlobalEmergencyToggle_ActiveTooltip".Translate()
                    : "DS_GlobalEmergencyToggle_InactiveTooltip".Translate(),
                SoundDefOf.Mouseover_ButtonToggle);

            if (requestedState != active)
            {
                component.TrySetGlobalEmergencyEvacuation(map, requestedState);
            }
        }
    }
}
