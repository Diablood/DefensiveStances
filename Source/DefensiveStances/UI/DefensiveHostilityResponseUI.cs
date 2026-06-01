using System.Collections.Generic;
using DefensiveStances.Components;
using DefensiveStances.Domain;
using DefensiveStances.Utilities;
using RimWorld;
using UnityEngine;
using Verse;

namespace DefensiveStances.UI
{
    [StaticConstructorOnStartup]
    internal static class DefensiveHostilityResponseUI
    {
        private enum DefensiveResponseSelection
        {
            Ignore,
            Attack,
            Flee,
            FleeToSafeArea,
            SelfDefenseOnly
        }

        private static readonly Color IconColor = new Color(0.84f, 0.84f, 0.84f);
        private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore");
        private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack");
        private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee");

        internal static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
        {
            DefensiveResponseSelection selection = GetSelection(pawn);
            Widgets.Dropdown<Pawn, DefensiveResponseSelection>(
                rect,
                pawn,
                IconColor,
                GetSelection,
                GenerateMenu,
                null,
                GetIcon(selection),
                null,
                null,
                delegate
                {
                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
                },
                paintable,
                4f);

            UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
            if (Mouse.IsOver(rect))
            {
                TooltipHandler.TipRegion(
                    rect,
                    "HostilityReponseTip".Translate() + "\n\n" +
                    "HostilityResponseCurrentMode".Translate() + ": " + GetLabel(selection));
            }
        }

        private static DefensiveResponseSelection GetSelection(Pawn pawn)
        {
            DefensivePawnState state = DefensiveStancesGameComponent.Current?.GetPawnState(pawn, false);
            if (state?.mode == DefensiveBehaviorMode.FleeToSafeArea)
            {
                return DefensiveResponseSelection.FleeToSafeArea;
            }

            if (state?.mode == DefensiveBehaviorMode.SelfDefenseOnly)
            {
                return DefensiveResponseSelection.SelfDefenseOnly;
            }

            switch (pawn.playerSettings.hostilityResponse)
            {
                case HostilityResponseMode.Ignore:
                    return DefensiveResponseSelection.Ignore;
                case HostilityResponseMode.Attack:
                    return DefensiveResponseSelection.Attack;
                default:
                    return DefensiveResponseSelection.Flee;
            }
        }

        private static IEnumerable<Widgets.DropdownMenuElement<DefensiveResponseSelection>> GenerateMenu(Pawn pawn)
        {
            yield return CreateMenuElement(pawn, DefensiveResponseSelection.Ignore);

            if (!pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                yield return CreateMenuElement(pawn, DefensiveResponseSelection.Attack);
            }

            yield return CreateMenuElement(pawn, DefensiveResponseSelection.Flee);
            yield return CreateMenuElement(pawn, DefensiveResponseSelection.FleeToSafeArea);

            if (!pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                yield return CreateMenuElement(pawn, DefensiveResponseSelection.SelfDefenseOnly);
            }
        }

        private static Widgets.DropdownMenuElement<DefensiveResponseSelection> CreateMenuElement(Pawn pawn, DefensiveResponseSelection selection)
        {
            return new Widgets.DropdownMenuElement<DefensiveResponseSelection>
            {
                option = new FloatMenuOption(
                    GetLabel(selection),
                    delegate { ApplySelection(pawn, selection); },
                    GetIcon(selection),
                    Color.white),
                payload = selection
            };
        }

        private static void ApplySelection(Pawn pawn, DefensiveResponseSelection selection)
        {
            DefensivePawnState state = DefensiveStancesGameComponent.Current?.GetPawnState(pawn);
            if (state == null)
            {
                return;
            }

            if (selection != DefensiveResponseSelection.FleeToSafeArea && state.evacuationActive)
            {
                DefensiveEvacuationUtility.RestorePreviousArea(state);
            }

            if (selection != DefensiveResponseSelection.SelfDefenseOnly)
            {
                state.ClearAggression();
            }

            switch (selection)
            {
                case DefensiveResponseSelection.Ignore:
                    state.mode = DefensiveBehaviorMode.Vanilla;
                    pawn.playerSettings.hostilityResponse = HostilityResponseMode.Ignore;
                    break;
                case DefensiveResponseSelection.Attack:
                    state.mode = DefensiveBehaviorMode.Vanilla;
                    pawn.playerSettings.hostilityResponse = HostilityResponseMode.Attack;
                    break;
                case DefensiveResponseSelection.Flee:
                    state.mode = DefensiveBehaviorMode.Vanilla;
                    pawn.playerSettings.hostilityResponse = HostilityResponseMode.Flee;
                    break;
                case DefensiveResponseSelection.FleeToSafeArea:
                    state.mode = DefensiveBehaviorMode.FleeToSafeArea;
                    DefensiveBehaviorUtility.ApplyClosestVanillaFallback(pawn, state.mode);
                    NotifyMissingSafeAreaOnSelection(pawn, state);
                    DefensiveEvacuationUtility.TryStartEvacuationForLocalDanger(state);
                    break;
                case DefensiveResponseSelection.SelfDefenseOnly:
                    state.mode = DefensiveBehaviorMode.SelfDefenseOnly;
                    DefensiveBehaviorUtility.ApplyClosestVanillaFallback(pawn, state.mode);
                    break;
            }
        }

        private static void NotifyMissingSafeAreaOnSelection(Pawn pawn, DefensivePawnState state)
        {
            if (!pawn.Spawned || pawn.Map == null)
            {
                return;
            }

            Area safeArea = DefensiveStancesGameComponent.Current?.GetSafeArea(pawn.Map);
            if (safeArea == null || safeArea.TrueCount <= 0)
            {
                DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoSafeArea);
            }
        }

        private static string GetLabel(DefensiveResponseSelection selection)
        {
            switch (selection)
            {
                case DefensiveResponseSelection.Ignore:
                    return HostilityResponseMode.Ignore.GetLabel();
                case DefensiveResponseSelection.Attack:
                    return HostilityResponseMode.Attack.GetLabel();
                case DefensiveResponseSelection.Flee:
                    return HostilityResponseMode.Flee.GetLabel();
                case DefensiveResponseSelection.FleeToSafeArea:
                    return "DS_Mode_FleeToSafeArea".Translate();
                case DefensiveResponseSelection.SelfDefenseOnly:
                    return "DS_Mode_SelfDefenseOnly".Translate();
                default:
                    return "DS_Mode_Vanilla".Translate();
            }
        }

        private static Texture2D GetIcon(DefensiveResponseSelection selection)
        {
            switch (selection)
            {
                case DefensiveResponseSelection.Ignore:
                    return IgnoreIcon;
                case DefensiveResponseSelection.Attack:
                case DefensiveResponseSelection.SelfDefenseOnly:
                    return AttackIcon;
                case DefensiveResponseSelection.Flee:
                case DefensiveResponseSelection.FleeToSafeArea:
                    return FleeIcon;
                default:
                    return BaseContent.BadTex;
            }
        }
    }
}
