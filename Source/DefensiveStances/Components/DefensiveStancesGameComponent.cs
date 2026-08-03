using System.Collections.Generic;
using DefensiveStances.Areas;
using DefensiveStances.Domain;
using DefensiveStances.Settings;
using DefensiveStances.Utilities;
using RimWorld;
using Verse;

namespace DefensiveStances.Components
{
    internal sealed class DefensiveStancesGameComponent : GameComponent
    {
        private List<DefensivePawnState> pawnStates = new List<DefensivePawnState>();
        private List<DefensiveMapState> mapStates = new List<DefensiveMapState>();
        private List<GlobalEmergencyEvacuationMapState> globalEmergencyEvacuationStates = new List<GlobalEmergencyEvacuationMapState>();

        internal static DefensiveStancesGameComponent Current => Verse.Current.Game?.GetComponent<DefensiveStancesGameComponent>();

        public DefensiveStancesGameComponent(Game game)
        {
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref pawnStates, "pawnStates", LookMode.Deep);
            Scribe_Collections.Look(ref mapStates, "mapStates", LookMode.Deep);
            Scribe_Collections.Look(ref globalEmergencyEvacuationStates, "globalEmergencyEvacuationStates", LookMode.Deep);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                pawnStates = pawnStates ?? new List<DefensivePawnState>();
                mapStates = mapStates ?? new List<DefensiveMapState>();
                globalEmergencyEvacuationStates = globalEmergencyEvacuationStates ?? new List<GlobalEmergencyEvacuationMapState>();
                pawnStates.RemoveAll(state => state == null || state.pawn == null);
                mapStates.RemoveAll(state => state == null || state.map == null);
                globalEmergencyEvacuationStates.RemoveAll(state => state == null || state.map == null);
            }
        }

        public override void LoadedGame()
        {
            EnforceActiveGlobalEmergencyEvacuations();
        }

        public override void GameComponentTick()
        {
            if (!GenTicks.IsTickInterval(DefensiveStancesSettings.Current.containmentCheckIntervalTicks))
            {
                return;
            }

            EnforceActiveGlobalEmergencyEvacuations();

            for (int index = pawnStates.Count - 1; index >= 0; index--)
            {
                DefensivePawnState state = pawnStates[index];
                if (state == null || state.pawn == null)
                {
                    pawnStates.RemoveAt(index);
                    continue;
                }

                MaintainEvacuation(state);
            }
        }

        internal DefensivePawnState GetPawnState(Pawn pawn, bool createIfMissing = true)
        {
            if (pawn == null)
            {
                return null;
            }

            DefensivePawnState state = pawnStates.Find(candidate => candidate.pawn == pawn);
            if (state == null && createIfMissing)
            {
                state = new DefensivePawnState { pawn = pawn };
                pawnStates.Add(state);
            }

            return state;
        }

        internal Area GetSafeArea(Map map)
        {
            Area_Safe safeArea = DefensiveSafeAreaUtility.GetOrCreate(map);
            MigrateLegacySafeArea(map, safeArea);
            return safeArea;
        }

        internal bool IsGlobalEmergencyEvacuationActive(Map map)
        {
            return GetGlobalEmergencyEvacuationState(map, false)?.active == true;
        }

        internal bool TrySetGlobalEmergencyEvacuation(Map map, bool active)
        {
            if (map == null)
            {
                return false;
            }

            GlobalEmergencyEvacuationMapState mapState = GetGlobalEmergencyEvacuationState(map, active);
            if (!active)
            {
                if (mapState == null || !mapState.active)
                {
                    return true;
                }

                mapState.active = false;
                ReleaseGlobalEmergencyEvacuation(map);
                DS_Log.Message("Global emergency evacuation disabled on the current map.");
                return true;
            }

            Area safeArea = GetSafeArea(map);
            if (safeArea == null || safeArea.TrueCount <= 0)
            {
                Messages.Message(
                    "DS_Message_GlobalEmergencyNoSafeArea".Translate(),
                    MessageTypeDefOf.RejectInput,
                    historical: false);
                DS_Log.Warning("Global emergency evacuation could not be enabled because the current map has no configured safe cells.");
                return false;
            }

            if (mapState.active)
            {
                return true;
            }

            mapState.active = true;
            DS_Log.Message("Global emergency evacuation enabled on the current map.");
            EnforceGlobalEmergencyEvacuation(map);
            return true;
        }

        internal void NotifyPawnUndrafted(Pawn pawn)
        {
            if (!DefensiveGlobalEmergencyEvacuationUtility.IsControllablePawn(pawn)
                || pawn.Map == null
                || !IsGlobalEmergencyEvacuationActive(pawn.Map))
            {
                return;
            }

            DefensiveEvacuationUtility.TryStartGlobalEmergencyEvacuation(pawn, GetPawnState(pawn));
        }

        internal void NotifySafeAreaChanged(Map map, Area safeArea)
        {
            if (map == null || safeArea == null)
            {
                return;
            }

            for (int index = pawnStates.Count - 1; index >= 0; index--)
            {
                DefensivePawnState state = pawnStates[index];
                if (state?.evacuationActive == true
                    && state.pawn?.Map == map
                    && state.evacuationArea == safeArea)
                {
                    DefensiveEvacuationUtility.MaintainSafeAreaContainment(state);
                }
            }

            if (IsGlobalEmergencyEvacuationActive(map))
            {
                EnforceGlobalEmergencyEvacuation(map);
            }
        }

        private GlobalEmergencyEvacuationMapState GetGlobalEmergencyEvacuationState(Map map, bool createIfMissing)
        {
            if (map == null)
            {
                return null;
            }

            GlobalEmergencyEvacuationMapState state = globalEmergencyEvacuationStates.Find(candidate => candidate.map == map);
            if (state == null && createIfMissing)
            {
                state = new GlobalEmergencyEvacuationMapState { map = map };
                globalEmergencyEvacuationStates.Add(state);
            }

            return state;
        }

        private void EnforceActiveGlobalEmergencyEvacuations()
        {
            for (int index = globalEmergencyEvacuationStates.Count - 1; index >= 0; index--)
            {
                GlobalEmergencyEvacuationMapState state = globalEmergencyEvacuationStates[index];
                if (state?.map == null)
                {
                    globalEmergencyEvacuationStates.RemoveAt(index);
                    continue;
                }

                if (state.active)
                {
                    EnforceGlobalEmergencyEvacuation(state.map);
                }
            }
        }

        private void EnforceGlobalEmergencyEvacuation(Map map)
        {
            IReadOnlyList<Pawn> pawns = map?.mapPawns?.AllPawnsSpawned;
            if (pawns == null)
            {
                return;
            }

            for (int index = 0; index < pawns.Count; index++)
            {
                Pawn pawn = pawns[index];
                if (!DefensiveGlobalEmergencyEvacuationUtility.IsControllablePawn(pawn) || pawn.Drafted)
                {
                    continue;
                }

                DefensivePawnState state = GetPawnState(pawn);
                if (pawn.Downed)
                {
                    DefensiveEvacuationFeedback.NotifyFailure(pawn, state, EvacuationFailureReason.NoReachableSafeCell);
                    continue;
                }

                DefensiveEvacuationUtility.TryStartGlobalEmergencyEvacuation(pawn, state);
            }
        }

        private void ReleaseGlobalEmergencyEvacuation(Map map)
        {
            for (int index = pawnStates.Count - 1; index >= 0; index--)
            {
                DefensivePawnState state = pawnStates[index];
                if (state?.pawn?.Map != map || !state.globalEmergencyEvacuationActive)
                {
                    continue;
                }

                DefensiveEvacuationUtility.ReleaseGlobalEmergencyEvacuation(state);
            }
        }

        private void MigrateLegacySafeArea(Map map, Area_Safe targetArea)
        {
            if (map == null || targetArea == null)
            {
                return;
            }

            for (int index = mapStates.Count - 1; index >= 0; index--)
            {
                DefensiveMapState legacyState = mapStates[index];
                if (legacyState?.map != map)
                {
                    continue;
                }

                if (legacyState.safeArea != null && legacyState.safeArea != targetArea)
                {
                    foreach (IntVec3 cell in legacyState.safeArea.ActiveCells)
                    {
                        targetArea[cell] = true;
                    }
                }

                mapStates.RemoveAt(index);
            }
        }

        private void MaintainEvacuation(DefensivePawnState state)
        {
            Pawn pawn = state.pawn;
            if (pawn?.playerSettings == null || !pawn.Spawned)
            {
                DefensiveEvacuationUtility.RestorePreviousArea(state);
                return;
            }

            bool mapEmergencyActive = IsGlobalEmergencyEvacuationActive(pawn.Map);
            if (!mapEmergencyActive)
            {
                state.globalEmergencyEvacuationActive = false;
            }

            if (!state.evacuationActive)
            {
                if (mapEmergencyActive && !pawn.Drafted)
                {
                    DefensiveEvacuationUtility.TryStartGlobalEmergencyEvacuation(pawn, state);
                }
                else
                {
                    DefensiveEvacuationUtility.TryStartEvacuationForLocalDanger(state);
                }

                return;
            }

            if (state.localDangerEvacuationActive)
            {
                if (state.mode != DefensiveBehaviorMode.FleeToSafeArea)
                {
                    state.localDangerEvacuationActive = false;
                }
                else if (DefensiveThreatUtility.ShouldKeepEvacuationActive(pawn))
                {
                    state.lastDangerTick = GenTicks.TicksGame;
                }
                else if (GenTicks.TicksGame - state.lastDangerTick >= DefensiveStancesSettings.Current.evacuationRestoreGraceTicks)
                {
                    state.localDangerEvacuationActive = false;
                }
            }

            if (!state.HasEvacuationReason)
            {
                DefensiveEvacuationUtility.RestorePreviousArea(state);
                return;
            }

            if (state.evacuationArea == null)
            {
                DefensiveEvacuationUtility.RestorePreviousArea(state);
                return;
            }

            Area currentAllowedArea = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
            if (currentAllowedArea == state.evacuationArea)
            {
                pawn.playerSettings.AreaRestrictionInPawnCurrentMap = state.previousAllowedArea;
                currentAllowedArea = state.previousAllowedArea;
            }

            if (currentAllowedArea != state.previousAllowedArea)
            {
                if (!state.globalEmergencyEvacuationActive || pawn.Drafted)
                {
                    // A manual player change wins over local doctrine containment.
                    state.ClearEvacuationTracking();
                    return;
                }

                state.previousAllowedArea = currentAllowedArea;
            }

            DefensiveEvacuationUtility.MaintainSafeAreaContainment(state);
        }
    }
}
