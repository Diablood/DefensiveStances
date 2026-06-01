using Verse;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveGlobalEmergencyEvacuationUtility
    {
        internal static bool IsControllablePawn(Pawn pawn)
        {
            return pawn?.Spawned == true
                && pawn.playerSettings != null
                && pawn.IsPlayerControlled;
        }
    }
}
