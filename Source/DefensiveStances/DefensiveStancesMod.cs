using DefensiveStances.Settings;
using UnityEngine;
using Verse;

namespace DefensiveStances
{
    public sealed class DefensiveStancesMod : Mod
    {
        internal static DefensiveStancesSettings Settings { get; private set; } = new DefensiveStancesSettings();

        public DefensiveStancesMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<DefensiveStancesSettings>() ?? new DefensiveStancesSettings();
            Settings.ClampToValidRange();
        }

        public override string SettingsCategory()
        {
            return "DS_Settings_Category".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("DS_Settings_Description".Translate());
            listing.GapLine();

            int restoreGraceSeconds = Mathf.RoundToInt(Settings.evacuationRestoreGraceTicks / 60f);
            listing.Label(
                "DS_Settings_RestoreGrace_Label".Translate(restoreGraceSeconds),
                tooltip: "DS_Settings_RestoreGrace_Tooltip".Translate());
            restoreGraceSeconds = Mathf.RoundToInt(listing.Slider(restoreGraceSeconds, 0f, 60f));
            Settings.evacuationRestoreGraceTicks = restoreGraceSeconds * 60;

            listing.Gap();

            float containmentCheckSeconds = Settings.containmentCheckIntervalTicks / 60f;
            listing.Label(
                "DS_Settings_CheckInterval_Label".Translate(containmentCheckSeconds.ToString("0.##")),
                tooltip: "DS_Settings_CheckInterval_Tooltip".Translate());
            containmentCheckSeconds = Mathf.Round(listing.Slider(containmentCheckSeconds, 0.25f, 5f) * 4f) / 4f;
            Settings.containmentCheckIntervalTicks = Mathf.RoundToInt(containmentCheckSeconds * 60f);

            listing.GapLine();

            listing.CheckboxLabeled(
                "DS_Settings_ShowWarningMessages_Label".Translate(),
                ref Settings.showWarningMessages,
                "DS_Settings_ShowWarningMessages_Tooltip".Translate());
            listing.CheckboxLabeled(
                "DS_Settings_AllowVanillaFallback_Label".Translate(),
                ref Settings.allowVanillaFleeFallback,
                "DS_Settings_AllowVanillaFallback_Tooltip".Translate());

            listing.Gap();

            if (listing.ButtonText("DS_Settings_ResetDefaults".Translate()))
            {
                Settings.ResetToDefaults();
            }

            listing.End();
        }

        public override void WriteSettings()
        {
            Settings.ClampToValidRange();
            base.WriteSettings();
        }
    }
}
