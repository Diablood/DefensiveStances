using DefensiveStances.Domain;
using RimWorld;
using Verse;
using Verse.AI;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveEvacuationUtility
    {
        private const int ContainmentRecoveryLogCooldownTicks = 600;

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
            if (!DefensiveSafeAreaUtility.TryFindReachableSafeCell(pawn, safeArea, out destination))
            {
                RestorePreviousAreaIfNecessary(state);
                DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoReachableSafeCell);
                return false;
            }

            StartOrRefreshEvacuation(pawn, state, safeArea);
            state.ClearEvacuationFailure();

            job = CreateGotoSafeAreaJob(destination);
            return true;
        }

        internal static void MaintainSafeAreaContainment(DefensivePawnState state)
        {
            Pawn pawn = state?.pawn;
            Area safeArea = state?.evacuationArea;
            if (pawn?.playerSettings == null || pawn.jobs == null || state?.evacuationActive != true || !pawn.Spawned)
            {
                return;
            }

            if (safeArea == null || safeArea.Map != pawn.Map || safeArea.TrueCount <= 0)
            {
                StopEvacuationMovementIfNecessary(pawn);
                RestorePreviousArea(state);
                DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoSafeArea);
                return;
            }

            if (pawn.Drafted || PawnUtility.PlayerForcedJobNowOrSoon(pawn))
            {
                // Direct player control wins temporarily. The safe-area restriction remains active,
                // and containment resumes when the forced order or drafted state ends.
                return;
            }

            if (safeArea[pawn.Position])
            {
                InterruptAutomaticMovementLeavingSafeArea(pawn, state, safeArea);
                return;
            }

            if (IsMovingTowardSafeArea(pawn, safeArea))
            {
                return;
            }

            IntVec3 destination;
            if (!DefensiveSafeAreaUtility.TryFindReachableSafeCell(pawn, safeArea, out destination))
            {
                StopEvacuationMovementIfNecessary(pawn);
                RestorePreviousArea(state);
                DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoReachableSafeCell);
                return;
            }

            pawn.jobs.StartJob(CreateGotoSafeAreaJob(destination), JobCondition.InterruptForced);
            LogContainmentRecovery(pawn, state, "redirected back into a safe area while evacuation remains active.");
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

        private static Job CreateGotoSafeAreaJob(IntVec3 destination)
        {
            Job job = JobMaker.MakeJob(JobDefOf.Goto, destination);
            job.reportStringOverride = "DS_Job_FleeToSafeArea_Report".Translate();
            job.expiryInterval = 600;
            return job;
        }

        private static bool IsMovingTowardSafeArea(Pawn pawn, Area safeArea)
        {
            return pawn.pather != null
                && pawn.pather.Moving
                && IsSafeCell(safeArea, pawn.pather.Destination.Cell);
        }

        private static void InterruptAutomaticMovementLeavingSafeArea(Pawn pawn, DefensivePawnState state, Area safeArea)
        {
            if (pawn.jobs?.curJob == null
                || pawn.jobs.curJob.playerForced
                || pawn.pather == null
                || !pawn.pather.Moving
                || IsSafeCell(safeArea, pawn.pather.Destination.Cell))
            {
                return;
            }

            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
            LogContainmentRecovery(pawn, state, "interrupted an automatic job that would leave the active safe area.");
        }

        private static void StopEvacuationMovementIfNecessary(Pawn pawn)
        {
            Job currentJob = pawn.jobs?.curJob;
            if (currentJob == null
                || currentJob.playerForced
                || currentJob.def != JobDefOf.Goto
                || currentJob.reportStringOverride != "DS_Job_FleeToSafeArea_Report".Translate())
            {
                return;
            }

            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
        }

        private static bool IsSafeCell(Area safeArea, IntVec3 cell)
        {
            return cell.IsValid
                && cell.InBounds(safeArea.Map)
                && safeArea[cell];
        }

        private static void LogContainmentRecovery(Pawn pawn, DefensivePawnState state, string message)
        {
            if (state.ShouldLogContainmentRecovery(ContainmentRecoveryLogCooldownTicks))
            {
                DS_Log.Message(pawn.LabelShortCap + " " + message);
            }
        }

        private static void RestorePreviousAreaIfNecessary(DefensivePawnState state)
        {
            if (state?.evacuationActive == true)
            {
                StopEvacuationMovementIfNecessary(state.pawn);
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
    }
}
