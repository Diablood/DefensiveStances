using System.Collections.Generic;
using System.Reflection;
using DefensiveStances.Alerts;
using HarmonyLib;
using RimWorld;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveAlertRegistrar
    {
        private static readonly FieldInfo AllAlertsField = AccessTools.Field(typeof(AlertsReadout), "AllAlerts");

        internal static void EnsureRegistered(AlertsReadout alertsReadout)
        {
            if (alertsReadout == null)
            {
                DS_Log.WarningOnce(
                    "Cannot register the safe-area alert because the RimWorld alerts readout is unavailable.",
                    74350101);
                return;
            }

            if (AllAlertsField == null)
            {
                DS_Log.ErrorOnce(
                    "Cannot register the safe-area alert because RimWorld.AlertsReadout.AllAlerts was not found.",
                    74350102);
                return;
            }

            List<Alert> alerts = AllAlertsField.GetValue(alertsReadout) as List<Alert>;
            if (alerts == null)
            {
                DS_Log.ErrorOnce(
                    "Cannot register the safe-area alert because RimWorld.AlertsReadout.AllAlerts is unavailable.",
                    74350103);
                return;
            }

            for (int index = 0; index < alerts.Count; index++)
            {
                if (alerts[index] is Alert_NoSafeAreaConfigured)
                {
                    return;
                }
            }

            alerts.Add(new Alert_NoSafeAreaConfigured());
            DS_Log.Warning(
                "The safe-area alert was not discovered automatically by RimWorld and has been registered explicitly.");
        }
    }
}
