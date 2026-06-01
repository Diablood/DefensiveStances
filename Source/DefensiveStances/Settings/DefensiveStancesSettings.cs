using Verse;

namespace DefensiveStances.Settings
{
    public sealed class DefensiveStancesSettings : ModSettings
    {
        internal const int DefaultEvacuationRestoreGraceTicks = 600;
        internal const int DefaultContainmentCheckIntervalTicks = 60;
        internal const bool DefaultShowWarningMessages = true;
        internal const bool DefaultAllowVanillaFleeFallback = true;

        internal const int MinEvacuationRestoreGraceTicks = 0;
        internal const int MaxEvacuationRestoreGraceTicks = 3600;
        internal const int MinContainmentCheckIntervalTicks = 15;
        internal const int MaxContainmentCheckIntervalTicks = 300;

        public int evacuationRestoreGraceTicks = DefaultEvacuationRestoreGraceTicks;
        public int containmentCheckIntervalTicks = DefaultContainmentCheckIntervalTicks;
        public bool showWarningMessages = DefaultShowWarningMessages;
        public bool allowVanillaFleeFallback = DefaultAllowVanillaFleeFallback;

        internal static DefensiveStancesSettings Current => DefensiveStancesMod.Settings;

        public override void ExposeData()
        {
            Scribe_Values.Look(
                ref evacuationRestoreGraceTicks,
                "evacuationRestoreGraceTicks",
                DefaultEvacuationRestoreGraceTicks);
            Scribe_Values.Look(
                ref containmentCheckIntervalTicks,
                "containmentCheckIntervalTicks",
                DefaultContainmentCheckIntervalTicks);
            Scribe_Values.Look(ref showWarningMessages, "showWarningMessages", DefaultShowWarningMessages);
            Scribe_Values.Look(ref allowVanillaFleeFallback, "allowVanillaFleeFallback", DefaultAllowVanillaFleeFallback);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                ClampToValidRange();
            }
        }

        internal void ResetToDefaults()
        {
            evacuationRestoreGraceTicks = DefaultEvacuationRestoreGraceTicks;
            containmentCheckIntervalTicks = DefaultContainmentCheckIntervalTicks;
            showWarningMessages = DefaultShowWarningMessages;
            allowVanillaFleeFallback = DefaultAllowVanillaFleeFallback;
        }

        internal void ClampToValidRange()
        {
            evacuationRestoreGraceTicks = Clamp(
                evacuationRestoreGraceTicks,
                MinEvacuationRestoreGraceTicks,
                MaxEvacuationRestoreGraceTicks);
            containmentCheckIntervalTicks = Clamp(
                containmentCheckIntervalTicks,
                MinContainmentCheckIntervalTicks,
                MaxContainmentCheckIntervalTicks);
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }
    }
}
