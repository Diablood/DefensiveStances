using RimWorld;
using Verse;

namespace DefensiveStances.Domain
{
    internal sealed class DefensivePawnState : IExposable
    {
        private const int SelfDefenseWindowTicks = 900;

        internal Pawn pawn;
        internal DefensiveBehaviorMode mode = DefensiveBehaviorMode.Vanilla;
        internal Thing lastAggressor;
        internal int lastAggressionTick = -1;
        internal bool evacuationActive;
        internal bool localDangerEvacuationActive;
        internal bool globalEmergencyEvacuationActive;
        internal Area previousAllowedArea;
        internal Area evacuationArea;
        internal int lastDangerTick = -1;
        internal EvacuationFailureReason lastEvacuationFailureReason = EvacuationFailureReason.None;
        internal int lastEvacuationFailureMessageTick = -1;
        internal int lastContainmentRecoveryLogTick = -1;

        internal bool HasEvacuationReason => localDangerEvacuationActive || globalEmergencyEvacuationActive;

        public void ExposeData()
        {
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_Values.Look(ref mode, "mode", DefensiveBehaviorMode.Vanilla);
            Scribe_References.Look(ref lastAggressor, "lastAggressor");
            Scribe_Values.Look(ref lastAggressionTick, "lastAggressionTick", -1);
            Scribe_Values.Look(ref evacuationActive, "evacuationActive", false);
            Scribe_Values.Look(ref localDangerEvacuationActive, "localDangerEvacuationActive", false);
            Scribe_Values.Look(ref globalEmergencyEvacuationActive, "globalEmergencyEvacuationActive", false);
            Scribe_References.Look(ref previousAllowedArea, "previousAllowedArea");
            Scribe_References.Look(ref evacuationArea, "evacuationArea");
            Scribe_Values.Look(ref lastDangerTick, "lastDangerTick", -1);
            Scribe_Values.Look(ref lastContainmentRecoveryLogTick, "lastContainmentRecoveryLogTick", -1);

            if (Scribe.mode == LoadSaveMode.PostLoadInit
                && evacuationActive
                && !localDangerEvacuationActive
                && !globalEmergencyEvacuationActive)
            {
                // Saves created before 0.8.0 only had local doctrine evacuations.
                localDangerEvacuationActive = true;
            }
        }

        internal void RecordAggression(Thing aggressor)
        {
            lastAggressor = aggressor;
            lastAggressionTick = GenTicks.TicksGame;
        }

        internal bool TryGetRecentAggressor(Pawn defender, out Thing aggressor)
        {
            aggressor = lastAggressor;
            if (aggressor == null || aggressor.Destroyed || !aggressor.Spawned)
            {
                ClearAggression();
                return false;
            }

            if (!defender.Spawned || aggressor.Map != defender.Map)
            {
                ClearAggression();
                return false;
            }

            if (GenTicks.TicksGame - lastAggressionTick > SelfDefenseWindowTicks)
            {
                ClearAggression();
                return false;
            }

            if (!GenHostility.HostileTo(aggressor, defender))
            {
                ClearAggression();
                return false;
            }

            return true;
        }

        internal void ClearAggression()
        {
            lastAggressor = null;
            lastAggressionTick = -1;
        }

        internal bool ShouldReportEvacuationFailure(EvacuationFailureReason reason, int cooldownTicks)
        {
            if (reason == EvacuationFailureReason.None)
            {
                return false;
            }

            int currentTick = GenTicks.TicksGame;
            bool shouldReport = lastEvacuationFailureReason != reason
                || lastEvacuationFailureMessageTick < 0
                || currentTick - lastEvacuationFailureMessageTick >= cooldownTicks;

            if (shouldReport)
            {
                lastEvacuationFailureReason = reason;
                lastEvacuationFailureMessageTick = currentTick;
            }

            return shouldReport;
        }

        internal void ClearEvacuationFailure()
        {
            lastEvacuationFailureReason = EvacuationFailureReason.None;
            lastEvacuationFailureMessageTick = -1;
        }

        internal bool ShouldLogContainmentRecovery(int cooldownTicks)
        {
            int currentTick = GenTicks.TicksGame;
            if (lastContainmentRecoveryLogTick >= 0
                && currentTick - lastContainmentRecoveryLogTick < cooldownTicks)
            {
                return false;
            }

            lastContainmentRecoveryLogTick = currentTick;
            return true;
        }

        internal void ClearEvacuationTracking()
        {
            evacuationActive = false;
            localDangerEvacuationActive = false;
            globalEmergencyEvacuationActive = false;
            previousAllowedArea = null;
            evacuationArea = null;
            lastDangerTick = -1;
            lastContainmentRecoveryLogTick = -1;
        }
    }
}
