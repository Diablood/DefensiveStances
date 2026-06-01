using DefensiveStances.Areas;
using DefensiveStances.Utilities;
using RimWorld;
using UnityEngine;
using Verse;

namespace DefensiveStances.Designators
{
    public abstract class Designator_AreaSafe : Designator_Cells
    {
        private readonly DesignateMode mode;

        public override bool DragDrawMeasurements => true;

        public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.Areas;

        protected Designator_AreaSafe(DesignateMode mode)
        {
            this.mode = mode;
            soundDragSustain = SoundDefOf.Designate_DragStandard;
            soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
            useMouseIcon = true;
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds(Map) || c.Fogged(Map))
            {
                return false;
            }

            Area_Safe safeArea = DefensiveSafeAreaUtility.GetOrCreate(Map);
            if (safeArea == null)
            {
                return false;
            }

            bool alreadyIncluded = safeArea[c];
            return mode == DesignateMode.Add ? !alreadyIncluded : alreadyIncluded;
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            Area_Safe safeArea = DefensiveSafeAreaUtility.GetOrCreate(Map);
            if (safeArea != null)
            {
                safeArea[c] = mode == DesignateMode.Add;
                DefensiveStances.Components.DefensiveStancesGameComponent.Current?.NotifySafeAreaChanged(Map, safeArea);
            }
        }

        public override void SelectedUpdate()
        {
            GenUI.RenderMouseoverBracket();
            DefensiveSafeAreaUtility.GetOrCreate(Map)?.MarkForDraw();
        }
    }

    public sealed class Designator_AreaSafeExpand : Designator_AreaSafe
    {
        public Designator_AreaSafeExpand() : base(DesignateMode.Add)
        {
            defaultLabel = "DS_DesignatorSafeAreaExpand_Label".Translate();
            defaultDesc = "DS_DesignatorSafeAreaExpand_Desc".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/Designators/SafeAreaExpand");
            soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
            soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
            soundSucceeded = SoundDefOf.Designate_ZoneAdd;
            tutorTag = "DS_SafeAreaExpand";
        }
    }

    public sealed class Designator_AreaSafeClear : Designator_AreaSafe
    {
        public Designator_AreaSafeClear() : base(DesignateMode.Remove)
        {
            defaultLabel = "DS_DesignatorSafeAreaClear_Label".Translate();
            defaultDesc = "DS_DesignatorSafeAreaClear_Desc".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/Designators/SafeAreaClear");
            soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
            soundDragChanged = null;
            soundSucceeded = SoundDefOf.Designate_ZoneDelete;
            tutorTag = "DS_SafeAreaClear";
        }
    }
}
