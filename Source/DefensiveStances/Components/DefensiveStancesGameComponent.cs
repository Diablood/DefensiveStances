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

        internal static DefensiveStancesGameComponent Current => Verse.Current.Game?.GetComponent<DefensiveStancesGameComponent>();

        public DefensiveStancesGameComponent(Game game)
        {
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref pawnStates, "pawnStates", LookMode.Deep);
            Scribe_Collections.Look(ref mapStates, "mapStates", LookMode.Deep);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                pawnStates = pawnStates ?? new List<DefensivePawnState>();
                mapStates = mapStates ?? new List<DefensiveMapState>();
                pawnStates.RemoveAll(state => state == null || state.pawn == null);
                mapStates.RemoveAll(state => state == null || state.map == null);
            }
        }

        public override void GameComponentTick()
        {
            if (!GenTicks.IsTickInterval(DefensiveStancesSettings.Current.containmentCheckIntervalTicks))
            {
                return;
            }

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

        private static void MaintainEvacuation(DefensivePawnState state)
        {
            if (!state.evacuationActive)
            {
                return;
            }

            Pawn pawn = state.pawn;
            if (pawn.playerSettings == null || !pawn.Spawned || state.mode != DefensiveBehaviorMode.FleeToSafeArea)
            {
                DefensiveEvacuationUtility.RestorePreviousArea(state);
                return;
            }

            if (state.evacuationArea == null || pawn.playerSettings.AreaRestrictionInPawnCurrentMap != state.evacuationArea)
            {
                // A manual player change wins over automated restoration.
                state.ClearEvacuationTracking();
                return;
            }

            if (DefensiveThreatUtility.ShouldKeepEvacuationActive(pawn))
            {
                state.lastDangerTick = GenTicks.TicksGame;
            }
            else if (GenTicks.TicksGame - state.lastDangerTick >= DefensiveStancesSettings.Current.evacuationRestoreGraceTicks)
            {
                DefensiveEvacuationUtility.RestorePreviousArea(state);
                return;
            }

            DefensiveEvacuationUtility.MaintainSafeAreaContainment(state);
        }
    }
}
