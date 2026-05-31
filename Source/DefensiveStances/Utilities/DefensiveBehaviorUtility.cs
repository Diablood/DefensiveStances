using DefensiveStances.Domain;
using RimWorld;
using Verse;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveBehaviorUtility
    {
        internal static bool CanConfigure(Pawn pawn)
        {
            return pawn != null
                && pawn.Spawned
                && pawn.playerSettings != null
                && pawn.playerSettings.UsesConfigurableHostilityResponse;
        }

        internal static void ApplyClosestVanillaFallback(Pawn pawn, DefensiveBehaviorMode mode)
        {
            if (pawn?.playerSettings == null)
            {
                return;
            }

            if (mode == DefensiveBehaviorMode.FleeToSafeArea)
            {
                pawn.playerSettings.hostilityResponse = HostilityResponseMode.Flee;
            }
            else if (mode == DefensiveBehaviorMode.SelfDefenseOnly)
            {
                pawn.playerSettings.hostilityResponse = HostilityResponseMode.Ignore;
            }
        }
    }
}
