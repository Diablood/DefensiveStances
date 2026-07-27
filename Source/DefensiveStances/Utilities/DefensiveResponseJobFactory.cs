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

            if (verb.IsMeleeAttack || pawn.CanReachImmediate(aggressor, PathEndMode.Touch))
            {
                return JobMaker.MakeJob(JobDefOf.AttackMelee, aggressor);
            }

            if (verb.ApparelPreventsShooting())
            {
                return null;
            }

            Job job = JobMaker.MakeJob(JobDefOf.AttackStatic, aggressor);
            job.maxNumStaticAttacks = 2;
            job.expiryInterval = 600;
            job.endIfCantShootTargetFromCurPos = true;
            return job;
        }
    }
}
