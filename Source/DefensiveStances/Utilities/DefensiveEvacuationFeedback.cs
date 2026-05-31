using DefensiveStances.Domain;
using RimWorld;
using Verse;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveEvacuationFeedback
    {
        private const int FeedbackCooldownTicks = 600;

        internal static void NotifyFailure(Pawn pawn, DefensivePawnState state, EvacuationFailureReason reason)
        {
            if (pawn == null || state == null || !state.ShouldReportEvacuationFailure(reason, FeedbackCooldownTicks))
            {
                return;
            }

            string playerMessage;
            string logMessage;

            switch (reason)
            {
                case EvacuationFailureReason.NoSafeArea:
                    playerMessage = "DS_Message_NoSafeAreaConfigured".Translate();
                    logMessage = "could not flee to a safe area because no safe cells are configured.";
                    break;
                case EvacuationFailureReason.NoReachableSafeCell:
                    playerMessage = "DS_Message_NoReachableSafeCell".Translate();
                    logMessage = "could not flee to a safe area because no configured safe cell is reachable.";
                    break;
                default:
                    return;
            }

            Messages.Message(
                pawn.LabelShortCap + ": " + playerMessage,
                new LookTargets(pawn),
                MessageTypeDefOf.CautionInput,
                historical: false);

            DS_Log.Warning(pawn.LabelShortCap + " " + logMessage);
        }
    }
}
