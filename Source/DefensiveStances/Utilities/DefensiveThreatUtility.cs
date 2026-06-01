using RimWorld;
using Verse;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveThreatUtility
    {
        /// <summary>
        /// Keeps an already-started evacuation active while either the pawn still has a local
        /// reason to flee or the map contains any active hostile threat to the player.
        /// Initial evacuation remains local and is still started by the vanilla fleeing check.
        /// </summary>
        internal static bool ShouldKeepEvacuationActive(Pawn pawn)
        {
            if (pawn?.Map == null)
            {
                return false;
            }

            return SelfDefenseUtility.ShouldStartFleeing(pawn)
                || GenHostility.AnyHostileActiveThreatToPlayer(pawn.Map);
        }
    }
}
