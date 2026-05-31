using DefensiveStances.Areas;
using Verse;

namespace DefensiveStances.Utilities
{
    internal static class DefensiveSafeAreaUtility
    {
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
    }
}
