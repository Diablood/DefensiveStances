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

        public Alert_NoSafeAreaConfigured()
        {
            defaultPriority = AlertPriority.Medium;
            defaultLabel = "DS_Alert_NoSafeArea_Label".Translate();
            defaultExplanation = "DS_Alert_NoSafeArea_Explanation".Translate();
        }

        public override AlertReport GetReport()
        {
            culprits.Clear();

            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            if (component == null)
            {
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

            return AlertReport.CulpritsAre(culprits);
        }
    }
}
