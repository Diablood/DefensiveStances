using DefensiveStances.Domain;
using RimWorld;
using Verse;
using Verse.AI;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveResponseJobFactory
    {
        internal static Job TryCreateSelfDefenseJob(Pawn pawn, DefensivePawnState state)
        {
            if (pawn.Drafted)
            {
                state.ClearAggression();
                return null;
            }

            if (pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                return null;
            }

            Thing aggressor;
            if (!state.TryGetRecentAggressor(pawn, out aggressor))
            {
                return null;
            }

            Verb verb = pawn.TryGetAttackVerb(aggressor, !pawn.IsColonist);
            if (verb == null)
            {
                return null;
            }

            bool aggressorInsideAllowedArea = IsInsideVanillaAllowedArea(pawn, aggressor);

            if (verb.IsMeleeAttack)
            {
                if (!aggressorInsideAllowedArea)
                {
                    return null;
                }

                return JobMaker.MakeJob(JobDefOf.AttackMelee, aggressor);
            }

            if (pawn.CanReachImmediate(aggressor, PathEndMode.Touch) && aggressorInsideAllowedArea)
            {
                return JobMaker.MakeJob(JobDefOf.AttackMelee, aggressor);
            }

            if (verb.ApparelPreventsShooting() || !verb.CanHitTarget(aggressor))
            {
                return null;
            }

            Job job = JobMaker.MakeJob(JobDefOf.AttackStatic, aggressor);
            job.maxNumStaticAttacks = 2;
            job.expiryInterval = 600;
            job.endIfCantShootTargetFromCurPos = true;
            return job;
        }

        private static bool IsInsideVanillaAllowedArea(Pawn pawn, Thing target)
        {
            Area allowedArea = pawn?.playerSettings?.AreaRestrictionInPawnCurrentMap;
            if (allowedArea == null)
            {
                return true;
            }

            return target != null
                && target.Spawned
                && target.Map == pawn.Map
                && target.Position.IsValid
                && target.Position.InBounds(pawn.Map)
                && allowedArea[target.Position];
        }
    }
}
