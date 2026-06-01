using System.Collections.Generic;
using DefensiveStances.Components;
using DefensiveStances.Domain;
using DefensiveStances.Utilities;
using RimWorld;
using Verse;

namespace DefensiveStances.Alerts
{
    public sealed class Alert_NoReachableSafeArea : Alert
    {
        private readonly List<Pawn> culprits = new List<Pawn>();
        private bool activationLogged;

        public Alert_NoReachableSafeArea()
        {
            defaultPriority = AlertPriority.Medium;
            defaultLabel = "DS_Alert_NoReachableSafeArea_Label".Translate();
            defaultExplanation = "DS_Alert_NoReachableSafeArea_Explanation".Translate();
            DS_Log.Message("Safe-area reachability alert initialized.");
        }

        public override AlertReport GetReport()
        {
            culprits.Clear();

            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            if (component == null)
            {
                DS_Log.WarningOnce(
                    "Cannot evaluate the safe-area reachability alert because the game component is unavailable.",
                    74350105);
                return AlertReport.Inactive;
            }

            foreach (Map map in Find.Maps)
            {
                Area safeArea = component.GetSafeArea(map);
                if (safeArea == null || safeArea.TrueCount <= 0)
                {
                    // The empty-layer case is handled by Alert_NoSafeAreaConfigured.
                    continue;
                }

                bool globalEmergencyActive = component.IsGlobalEmergencyEvacuationActive(map);
                IReadOnlyList<Pawn> spawnedPawns = map.mapPawns.AllPawnsSpawned;
                for (int index = 0; index < spawnedPawns.Count; index++)
                {
                    Pawn pawn = spawnedPawns[index];
                    DefensivePawnState state = component.GetPawnState(pawn, false);
                    bool shouldEvaluate = (DefensiveBehaviorUtility.CanConfigure(pawn)
                            && state?.mode == DefensiveBehaviorMode.FleeToSafeArea)
                        || (globalEmergencyActive
                            && !pawn.Drafted
                            && DefensiveGlobalEmergencyEvacuationUtility.IsControllablePawn(pawn));
                    if (!shouldEvaluate)
                    {
                        continue;
                    }

                    IntVec3 destination;
                    if (pawn.Downed || !DefensiveSafeAreaUtility.TryFindReachableSafeCell(pawn, safeArea, out destination))
                    {
                        culprits.Add(pawn);
                    }
                }
            }

            if (culprits.Count > 0)
            {
                if (!activationLogged)
                {
                    activationLogged = true;
                    DS_Log.Message("Safe-area reachability alert activated for " + culprits.Count + " pawn(s).");
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
