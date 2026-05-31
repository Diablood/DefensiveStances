using System.Collections.Generic;
using DefensiveStances.Components;
using DefensiveStances.Domain;
using DefensiveStances.Utilities;
using UnityEngine;
using Verse;

namespace DefensiveStances.UI
{
    internal static class DefensiveStancesGizmoFactory
    {
        internal static IEnumerable<Gizmo> CreateFor(Pawn pawn)
        {
            DefensiveStancesGameComponent component = DefensiveStancesGameComponent.Current;
            DefensivePawnState state = component?.GetPawnState(pawn);
            if (state == null)
            {
                yield break;
            }

            yield return new Command_Action
            {
                defaultLabel = "DS_ModeCommand_Label".Translate(),
                defaultDesc = "DS_ModeCommand_Desc".Translate(state.mode.Label()),
                icon = BaseContent.BadTex,
                action = delegate
                {
                    state.mode = state.mode.Next();
                    DefensiveBehaviorUtility.ApplyClosestVanillaFallback(pawn, state.mode);
                }
            };

            yield return new Command_Action
            {
                defaultLabel = "DS_SafeAreaCommand_Label".Translate(),
                defaultDesc = "DS_SafeAreaCommand_Desc".Translate(component.GetSafeArea(pawn.Map)?.Label ?? "DS_SafeArea_None".Translate()),
                icon = BaseContent.BadTex,
                action = delegate { OpenSafeAreaMenu(pawn, component); }
            };
        }

        private static void OpenSafeAreaMenu(Pawn pawn, DefensiveStancesGameComponent component)
        {
            List<FloatMenuOption> options = new List<FloatMenuOption>
            {
                new FloatMenuOption("DS_SafeArea_Clear".Translate(), delegate { component.SetSafeArea(pawn.Map, null); })
            };

            foreach (Area area in pawn.Map.areaManager.AllAreas)
            {
                if (!area.AssignableAsAllowed())
                {
                    continue;
                }

                Area capturedArea = area;
                options.Add(new FloatMenuOption(capturedArea.Label, delegate { component.SetSafeArea(pawn.Map, capturedArea); }));
            }

            if (options.Count == 1)
            {
                options.Add(new FloatMenuOption("DS_SafeArea_NoAssignableAreas".Translate(), null));
            }

            Find.WindowStack.Add(new FloatMenu(options));
        }
    }
}
