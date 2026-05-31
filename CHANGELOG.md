# Changelog

## 0.1.2-dev

- Fixed compilation against RimWorld 1.6: qualify `Verse.Current.Game` explicitly in the game-component accessor to avoid a collision with `DefensiveStancesGameComponent.Current`.


## 0.1.1-dev

- Fixed compilation against RimWorld 1.6: import and call `RimWorld.GenHostility.HostileTo` explicitly when validating an aggressor.

## 0.1.0-dev

- Create the RimWorld 1.6 repository scaffold.
- Add Harmony bootstrap and metadata dependency.
- Add saved per-pawn doctrines and per-map safe-area state.
- Add initial pawn gizmos.
- Add evacuation restriction and restoration logic.
- Add direct-damage aggressor tracking for self-defense-only.
- Add initial English and French translations.
