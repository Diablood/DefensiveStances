using DefensiveStances.Domain;
using RimWorld;
using Verse;
using Verse.AI;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveEvacuationUtility
    {
        internal static bool TryCreateEvacuationJob(Pawn pawn, DefensivePawnState state, Area safeArea, out Job job)
        {
            job = null;
            if (pawn?.playerSettings == null || state == null || safeArea == null || safeArea.Map != pawn.Map)
            {
                return false;
            }

            if (safeArea.TrueCount <= 0)
            {
                RestorePreviousAreaIfNecessary(state);
                DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoSafeArea);
                return false;
            }

            if (safeArea[pawn.Position])
            {
                StartOrRefreshEvacuation(pawn, state, safeArea);
                state.ClearEvacuationFailure();
                return true;
            }

            IntVec3 destination;
            if (!TryFindReachableSafeCell(pawn, safeArea, out destination))
            {
                RestorePreviousAreaIfNecessary(state);
                DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoReachableSafeCell);
                return false;
            }

            StartOrRefreshEvacuation(pawn, state, safeArea);
            state.ClearEvacuationFailure();

            job = JobMaker.MakeJob(JobDefOf.Goto, destination);
            job.reportStringOverride = "DS_Job_FleeToSafeArea_Report".Translate();
            job.expiryInterval = 600;
            return true;
        }

        internal static void RestorePreviousArea(DefensivePawnState state)
        {
            Pawn pawn = state?.pawn;
            if (pawn?.playerSettings != null && state.evacuationActive)
            {
                pawn.playerSettings.AreaRestrictionInPawnCurrentMap = state.previousAllowedArea;
            }

            state?.ClearEvacuationTracking();
        }

        private static void RestorePreviousAreaIfNecessary(DefensivePawnState state)
        {
            if (state?.evacuationActive == true)
            {
                RestorePreviousArea(state);
            }
        }

        private static void StartOrRefreshEvacuation(Pawn pawn, DefensivePawnState state, Area safeArea)
        {
            if (!state.evacuationActive)
            {
                state.previousAllowedArea = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
                state.evacuationArea = safeArea;
                state.evacuationActive = true;
            }

            state.lastDangerTick = GenTicks.TicksGame;
            pawn.playerSettings.AreaRestrictionInPawnCurrentMap = safeArea;
        }

        private static bool TryFindReachableSafeCell(Pawn pawn, Area safeArea, out IntVec3 destination)
        {
            destination = IntVec3.Invalid;
            int bestDistanceSquared = int.MaxValue;

            foreach (IntVec3 cell in safeArea.ActiveCells)
            {
                if (!cell.Standable(pawn.Map) || !pawn.CanReach(cell, PathEndMode.OnCell, Danger.Deadly))
                {
                    continue;
                }

                int distanceSquared = pawn.Position.DistanceToSquared(cell);
                if (distanceSquared < bestDistanceSquared)
                {
                    bestDistanceSquared = distanceSquared;
                    destination = cell;
                }
            }

            return destination.IsValid;
        }
    }
}
