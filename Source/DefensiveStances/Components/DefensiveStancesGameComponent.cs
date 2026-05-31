using System.Collections.Generic;
using DefensiveStances.Domain;
using DefensiveStances.Utilities;
using RimWorld;
using Verse;

namespace DefensiveStances.Components
{
    internal sealed class DefensiveStancesGameComponent : GameComponent
    {
        private const int EvacuationCheckIntervalTicks = 60;
        private const int EvacuationRestoreGraceTicks = 600;

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
            if (!GenTicks.IsTickInterval(EvacuationCheckIntervalTicks))
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
            return mapStates.Find(candidate => candidate.map == map)?.safeArea;
        }

        internal void SetSafeArea(Map map, Area safeArea)
        {
            if (map == null)
            {
                return;
            }

            DefensiveMapState state = mapStates.Find(candidate => candidate.map == map);
            if (state == null)
            {
                state = new DefensiveMapState { map = map };
                mapStates.Add(state);
            }

            state.safeArea = safeArea;
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

            if (SelfDefenseUtility.ShouldStartFleeing(pawn))
            {
                state.lastDangerTick = GenTicks.TicksGame;
                return;
            }

            if (GenTicks.TicksGame - state.lastDangerTick >= EvacuationRestoreGraceTicks)
            {
                DefensiveEvacuationUtility.RestorePreviousArea(state);
            }
        }
    }
}
