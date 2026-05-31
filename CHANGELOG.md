# Changelog

## 0.2.1-dev

- Display an explicit **Fleeing to a safe area** job report while a pawn is moving toward a painted shelter.
- Route melee-capable self-defense responses, including unarmed pawns, to `AttackMelee` instead of repeatedly creating an invalid static shooting job.
- Synchronize the mod metadata and assembly version to `0.2.1`.
- Keep the repository formatting convention at four spaces through `.editorconfig`.

## 0.2.0-dev

- Replace the two per-pawn gizmos with two entries inside RimWorld's existing hostility-response dropdown.
- Add a dedicated global safe-area layer for each map.
- Add **Expand safe area** and **Clear safe area** tools to the Architect **Zone** category.
- Allow safe-area cells to overlap stockpiles, growing zones and regular allowed areas.
- Keep disconnected shelters inside the same global map-level layer.
- Migrate cells from the legacy configured allowed area when a 0.1.x save first uses the new layer.
- Finalize the public metadata: author `Diablood`, package ID `diablood.defensivestances` and repository `Diablood/DefensiveStances`.

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
