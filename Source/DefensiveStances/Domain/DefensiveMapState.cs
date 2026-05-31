using Verse;

namespace DefensiveStances.Domain
{
    internal sealed class DefensiveMapState : IExposable
    {
        internal Map map;
        internal Area safeArea;

        public void ExposeData()
        {
            Scribe_References.Look(ref map, "map");
            Scribe_References.Look(ref safeArea, "safeArea");
        }
    }
}
