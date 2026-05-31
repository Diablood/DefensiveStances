using UnityEngine;
using Verse;

namespace DefensiveStances.Areas
{
    /// <summary>
    /// A global, map-level shelter layer. It deliberately behaves like a roof or home area:
    /// cells can overlap growing zones, stockpiles and regular allowed areas.
    /// </summary>
    public sealed class Area_Safe : Area
    {
        public override string Label => "DS_SafeArea_Label".Translate();

        public override Color Color => new Color(0.36f, 0.74f, 0.84f);

        public override int ListPriority => 8500;

        public Area_Safe()
        {
        }

        public Area_Safe(AreaManager areaManager) : base(areaManager)
        {
        }

        public override string GetUniqueLoadID()
        {
            return "Area_" + ID + "_DefensiveStancesSafe";
        }
    }
}
