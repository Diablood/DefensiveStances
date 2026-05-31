using Verse;

namespace DefensiveStances.Domain
{
    internal enum DefensiveBehaviorMode
    {
        Vanilla = 0,
        FleeToSafeArea = 1,
        SelfDefenseOnly = 2
    }

    internal static class DefensiveBehaviorModeExtensions
    {
        internal static TaggedString Label(this DefensiveBehaviorMode mode)
        {
            switch (mode)
            {
                case DefensiveBehaviorMode.FleeToSafeArea:
                    return "DS_Mode_FleeToSafeArea".Translate();
                case DefensiveBehaviorMode.SelfDefenseOnly:
                    return "DS_Mode_SelfDefenseOnly".Translate();
                default:
                    return "DS_Mode_Vanilla".Translate();
            }
        }

        internal static DefensiveBehaviorMode Next(this DefensiveBehaviorMode mode)
        {
            switch (mode)
            {
                case DefensiveBehaviorMode.Vanilla:
                    return DefensiveBehaviorMode.FleeToSafeArea;
                case DefensiveBehaviorMode.FleeToSafeArea:
                    return DefensiveBehaviorMode.SelfDefenseOnly;
                default:
                    return DefensiveBehaviorMode.Vanilla;
            }
        }
    }
}
