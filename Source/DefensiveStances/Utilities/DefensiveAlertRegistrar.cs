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
                    "Cannot register the safe-area alerts because the RimWorld alerts readout is unavailable.",
                    74350101);
                return;
            }

            if (AllAlertsField == null)
            {
                DS_Log.ErrorOnce(
                    "Cannot register the safe-area alerts because RimWorld.AlertsReadout.AllAlerts was not found.",
                    74350102);
                return;
            }

            List<Alert> alerts = AllAlertsField.GetValue(alertsReadout) as List<Alert>;
            if (alerts == null)
            {
                DS_Log.ErrorOnce(
                    "Cannot register the safe-area alerts because RimWorld.AlertsReadout.AllAlerts is unavailable.",
                    74350103);
                return;
            }

            bool addedMissingAlert = false;
            addedMissingAlert |= EnsureAlertRegistered<Alert_NoSafeAreaConfigured>(alerts);
            addedMissingAlert |= EnsureAlertRegistered<Alert_NoReachableSafeArea>(alerts);

            if (addedMissingAlert)
            {
                DS_Log.Warning(
                    "One or more safe-area alerts were not discovered automatically by RimWorld and have been registered explicitly.");
            }
        }

        private static bool EnsureAlertRegistered<TAlert>(List<Alert> alerts)
            where TAlert : Alert, new()
        {
            for (int index = 0; index < alerts.Count; index++)
            {
                if (alerts[index] is TAlert)
                {
                    return false;
                }
            }

            alerts.Add(new TAlert());
            return true;
        }
    }
}
