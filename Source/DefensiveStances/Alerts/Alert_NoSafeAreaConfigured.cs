using System.Collections.Generic;
using DefensiveStances.Components;
using DefensiveStances.Domain;
using RimWorld;
using Verse;

namespace DefensiveStances.Alerts
{
    public sealed class Alert_NoSafeAreaConfigured : Alert
    {
        private readonly List<Pawn> culprits = new List<Pawn>();
        private bool activationLogged;

        public Alert_NoSafeAreaConfigured()
        {
            defaultPriority = AlertPriority.Medium;
            defaultLabel = "DS_Alert_NoSafeArea_Label".Translate();
            defaultExplanation = "DS_Alert_NoSafeArea_Explanation".Translate();
            DS_Log.Message("Safe-area configuration alert initialized.");
        }

        public override AlertReport GetReport()
        {
            culprits.Clear();

            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            if (component == null)
            {
                DS_Log.WarningOnce(
                    "Cannot evaluate the safe-area alert because the game component is unavailable.",
                    74350104);
                return AlertReport.Inactive;
            }

            foreach (Map map in Find.Maps)
            {
                int firstMapCulpritIndex = culprits.Count;
                List<Pawn> freeColonists = map.mapPawns.FreeColonistsSpawned;

                for (int index = 0; index < freeColonists.Count; index++)
                {
                    Pawn pawn = freeColonists[index];
                    DefensivePawnState state = component.GetPawnState(pawn, false);
                    if (state?.mode == DefensiveBehaviorMode.FleeToSafeArea)
                    {
                        culprits.Add(pawn);
                    }
                }

                if (culprits.Count == firstMapCulpritIndex)
                {
                    continue;
                }

                Area safeArea = component.GetSafeArea(map);
                if (safeArea != null && safeArea.TrueCount > 0)
                {
                    culprits.RemoveRange(firstMapCulpritIndex, culprits.Count - firstMapCulpritIndex);
                }
            }

            if (culprits.Count > 0)
            {
                if (!activationLogged)
                {
                    activationLogged = true;
                    DS_Log.Message("Safe-area configuration alert activated for " + culprits.Count + " colonist(s).");
                }
            }
            else
            {
                activationLogged = false;
            }

            return AlertReport.CulpritsAre(culprits);
        }
    }
}
