using Verse;

namespace DefensiveStances.Domain
{
    internal sealed class GlobalEmergencyEvacuationMapState : IExposable
    {
        internal Map map;
        internal bool active;

        public void ExposeData()
        {
            Scribe_References.Look(ref map, "map");
            Scribe_Values.Look(ref active, "active", false);
        }
    }
}
