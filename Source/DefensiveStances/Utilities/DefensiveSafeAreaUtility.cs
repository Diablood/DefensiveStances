using DefensiveStances.Areas;
using Verse;
using Verse.AI;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveSafeAreaUtility
    {
        internal static Area_Safe Get(Map map)
        {
            return map?.areaManager?.Get<Area_Safe>();
        }

        internal static Area_Safe GetOrCreate(Map map)
        {
            if (map?.areaManager == null)
            {
                return null;
            }

            Area_Safe safeArea = map.areaManager.Get<Area_Safe>();
            if (safeArea != null)
            {
                return safeArea;
            }

            safeArea = new Area_Safe(map.areaManager);
            map.areaManager.AllAreas.Add(safeArea);
            return safeArea;
        }

        internal static bool TryFindReachableSafeCell(Pawn pawn, Area safeArea, out IntVec3 destination)
        {
            destination = IntVec3.Invalid;
            if (pawn == null || !pawn.Spawned || safeArea == null || safeArea.Map != pawn.Map)
            {
                return false;
            }

            if (safeArea[pawn.Position] && pawn.Position.Standable(pawn.Map))
            {
                destination = pawn.Position;
                return true;
            }

            int bestDistanceSquared = int.MaxValue;

            foreach (IntVec3 cell in safeArea.ActiveCells)
            {
                if (!cell.Standable(pawn.Map) || !pawn.CanReach(cell, PathEndMode.OnCell, Danger.Deadly))
                {
                    continue;
                }

                int distanceSquared = pawn.Position.DistanceToSquared(cell);
                if (distanceSquared < bestDistanceSquared)
                {
                    bestDistanceSquared = distanceSquared;
                    destination = cell;
                }
            }

            return destination.IsValid;
        }
    }
}
