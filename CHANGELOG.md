# Changelog

## 0.3.0-dev

- Add a persistent **No safe area configured** alert when a spawned free colonist uses **Flee to safe area** but their map has no painted safe cells.
- Show a throttled in-game warning focused on the affected pawn when no safe cell is configured or reachable during an evacuation attempt.
- Write the same evacuation failures through the colored `DS_Log.Warning` wrapper to simplify diagnostics.
- Avoid restricting a pawn to an unusable safe-area layer: validate reachability before starting evacuation and restore the previous allowed area if an active evacuation loses every viable shelter.
- Synchronize the mod metadata and assembly version to `0.3.0`.

## 0.2.2-dev

- Add the centralized `DS_Log` helper with `Message`, `Warning` and `Error` methods.
- Prefix mod logs with a colored **[Defensive Stances]** label for easier identification in RimWorld diagnostics.
- Route the Harmony bootstrap message through `DS_Log`.
- Synchronize the mod metadata and assembly version to `0.2.2`.

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
