using DefensiveStances.Components;
using DefensiveStances.Domain;
using DefensiveStances.Settings;
using RimWorld;
using Verse;
using Verse.AI;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveEvacuationUtility
    {
        private const int ContainmentRecoveryLogCooldownTicks = 600;

        internal static bool TryCreateEvacuationJob(
            Pawn pawn,
            DefensivePawnState state,
            Area safeArea,
            out Job job,
            bool globalEmergency = false)
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
                StartOrRefreshEvacuation(pawn, state, safeArea, globalEmergency);
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

            StartOrRefreshEvacuation(pawn, state, safeArea, globalEmergency);
            state.ClearEvacuationFailure();

            job = CreateGotoSafeAreaJob(destination);
            return true;
        }

        internal static bool TryStartEvacuationForLocalDanger(DefensivePawnState state)
        {
            Pawn pawn = state?.pawn;
            if (state?.evacuationActive != false || !CanInterruptForEvacuation(pawn, state, globalEmergency: false))
            {
                return false;
            }

            if (!SelfDefenseUtility.ShouldStartFleeing(pawn))
            {
                return false;
            }

            return TryStartImmediateEvacuation(pawn, state);
        }

        internal static bool TryStartImmediateEvacuation(Pawn pawn, DefensivePawnState state)
        {
            return TryStartImmediateEvacuation(pawn, state, globalEmergency: false);
        }

        internal static bool TryStartGlobalEmergencyEvacuation(Pawn pawn, DefensivePawnState state)
        {
            return TryStartImmediateEvacuation(pawn, state, globalEmergency: true);
        }

        internal static void ReleaseGlobalEmergencyEvacuation(DefensivePawnState state)
        {
            if (state == null)
            {
                return;
            }

            state.globalEmergencyEvacuationActive = false;
            if (!state.localDangerEvacuationActive)
            {
                StopEvacuationMovementIfNecessary(state.pawn);
                RestorePreviousArea(state);
            }
        }

        internal static void MaintainSafeAreaContainment(DefensivePawnState state)
        {
            Pawn pawn = state?.pawn;
            Area safeArea = state?.evacuationArea;
            if (pawn?.playerSettings == null || pawn.jobs == null || state?.evacuationActive != true || !pawn.Spawned)
            {
                return;
            }

            RestoreSafeAreaRestrictionIfApplied(state);

            if (safeArea == null || safeArea.Map != pawn.Map || safeArea.TrueCount <= 0)
            {
                StopEvacuationMovementIfNecessary(pawn);
                RestorePreviousArea(state);
                DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoSafeArea);
                return;
            }

            if (pawn.Drafted)
            {
                // Drafted pawns stay under direct player control. Global evacuation resumes as
                // soon as they are undrafted while the map-level alarm remains enabled.
                return;
            }

            if (!state.globalEmergencyEvacuationActive && PawnUtility.PlayerForcedJobNowOrSoon(pawn))
            {
                // Direct player control wins temporarily for doctrine-triggered evacuation only.
                return;
            }

            if (safeArea[pawn.Position])
            {
                InterruptMovementLeavingSafeArea(pawn, state, safeArea);
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
            RestoreSafeAreaRestrictionIfApplied(state);
            state?.ClearEvacuationTracking();
        }

        private static bool TryStartImmediateEvacuation(Pawn pawn, DefensivePawnState state, bool globalEmergency)
        {
            if (!CanInterruptForEvacuation(pawn, state, globalEmergency))
            {
                return false;
            }

            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            Area safeArea = component?.GetSafeArea(pawn.Map);
            Job evacuationJob;
            if (TryCreateEvacuationJob(pawn, state, safeArea, out evacuationJob, globalEmergency))
            {
                if (evacuationJob == null)
                {
                    MaintainSafeAreaContainment(state);
                    return true;
                }

                if (IsMovingTowardSafeArea(pawn, safeArea))
                {
                    JobMaker.ReturnToPool(evacuationJob);
                    return true;
                }

                pawn.jobs.StartJob(evacuationJob, JobCondition.InterruptForced);
                LogContainmentRecovery(
                    pawn,
                    state,
                    globalEmergency
                        ? "interrupted its current job to answer the global emergency evacuation alarm."
                        : "interrupted an automatic job to flee immediately into a safe area.");
                return true;
            }

            if (!globalEmergency
                && DefensiveStancesSettings.Current.allowVanillaFleeFallback
                && pawn.CurJob?.def != JobDefOf.FleeAndCower)
            {
                pawn.jobs.CheckForJobOverride();
            }

            return false;
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

        private static void InterruptMovementLeavingSafeArea(Pawn pawn, DefensivePawnState state, Area safeArea)
        {
            if (pawn.jobs?.curJob == null
                || (!state.globalEmergencyEvacuationActive && pawn.jobs.curJob.playerForced)
                || pawn.pather == null
                || !pawn.pather.Moving
                || IsSafeCell(safeArea, pawn.pather.Destination.Cell))
            {
                return;
            }

            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
            LogContainmentRecovery(pawn, state, "interrupted a job that would leave the active safe area.");
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

        private static bool CanInterruptForEvacuation(Pawn pawn, DefensivePawnState state, bool globalEmergency)
        {
            return pawn?.playerSettings != null
                && pawn.jobs != null
                && pawn.Spawned
                && !pawn.Downed
                && !pawn.Drafted
                && !pawn.jobs.startingNewJob
                && (globalEmergency || !PawnUtility.PlayerForcedJobNowOrSoon(pawn))
                && (globalEmergency || state?.mode == DefensiveBehaviorMode.FleeToSafeArea);
        }

        private static void RestorePreviousAreaIfNecessary(DefensivePawnState state)
        {
            if (state?.evacuationActive == true)
            {
                StopEvacuationMovementIfNecessary(state.pawn);
                RestorePreviousArea(state);
            }
        }

        private static void StartOrRefreshEvacuation(Pawn pawn, DefensivePawnState state, Area safeArea, bool globalEmergency)
        {
            if (!state.evacuationActive)
            {
                state.previousAllowedArea = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
                state.evacuationArea = safeArea;
                state.evacuationActive = true;
            }

            if (globalEmergency)
            {
                state.globalEmergencyEvacuationActive = true;
            }
            else
            {
                state.localDangerEvacuationActive = true;
                state.lastDangerTick = GenTicks.TicksGame;
            }

            RestoreSafeAreaRestrictionIfApplied(state);
        }

        private static void RestoreSafeAreaRestrictionIfApplied(DefensivePawnState state)
        {
            Pawn pawn = state?.pawn;
            if (pawn?.playerSettings == null
                || state?.evacuationActive != true
                || state.evacuationArea == null
                || pawn.playerSettings.AreaRestrictionInPawnCurrentMap != state.evacuationArea)
            {
                return;
            }

            pawn.playerSettings.AreaRestrictionInPawnCurrentMap = state.previousAllowedArea;
        }
    }
}
