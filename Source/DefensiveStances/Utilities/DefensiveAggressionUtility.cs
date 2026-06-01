using DefensiveStances.Components;
using DefensiveStances.Domain;
using RimWorld;
using Verse;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveAggressionUtility
    {
        internal static void RecordDirectAttack(Thing aggressor, Pawn defender)
        {
            if (defender == null || aggressor == null || aggressor == defender || !GenHostility.HostileTo(aggressor, defender))
            {
                return;
            }

            DefensivePawnState state = DefensiveStancesGameComponent.Current?.GetPawnState(defender, false);
            if (state == null)
            {
                return;
            }

            if (state.mode == DefensiveBehaviorMode.SelfDefenseOnly)
            {
                state.RecordAggression(aggressor);
                return;
            }

            if (state.mode == DefensiveBehaviorMode.FleeToSafeArea)
            {
                DefensiveEvacuationUtility.TryStartImmediateEvacuation(defender, state);
            }
        }
    }
}
