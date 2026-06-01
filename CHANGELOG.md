# Changelog

## 0.4.1-dev

- Add `tools/validate-translations.ps1` to validate the mod-owned English and French keyed translation files.
- Check XML parsing, duplicate keys, empty values, `TODO` placeholders and English/French key parity before compiling on Windows; run the same check from Bash when `pwsh` is available.
- Add `tools/find-translation-report.ps1` to locate RimWorld's generated `TranslationReport.txt` for the remaining Core-or-mod attribution pass.
- Document the translation-audit boundary: the repository validator covers Defensive Stances keys, while RimWorld's report also covers Core and any other loaded content.
- Synchronize the mod metadata and assembly version to `0.4.1`.

## 0.4.0-dev

- Trigger **Self-defense only** retaliation as soon as a hostile ranged projectile is launched directly at the pawn, even if the shot later misses or strikes cover.
- Trigger **Self-defense only** retaliation when a hostile melee attack is attempted, including misses and dodges.
- Centralize direct-aggressor recording in `DefensiveAggressionUtility` and keep `Thing.TakeDamage` as a fallback for other hostile damage sources.
- Add the 0.4 design note and extend the validation checklist for direct attack attempts.
- Record the French translation-report warning as a deferred audit item before a stable release.
- Synchronize the mod metadata and assembly version to `0.4.0`.

## 0.3.2-dev

- Add `[StaticConstructorOnStartup]` to `DefensiveHostilityResponseUI` so vanilla hostility-response textures are loaded on RimWorld's main thread.
- Include the loaded assembly version in the Harmony bootstrap log to make future DLL-version mismatches immediately visible.
- Synchronize the mod metadata and assembly version to `0.3.2`.

## 0.3.1-dev

- Register the safe-area configuration alert explicitly in RimWorld's `AlertsReadout` as a fallback when automatic alert discovery misses the custom alert class.
- Repeat the registration check from `UIRoot_Play.Init` so the alert is still injected if the readout was constructed before Harmony patches were applied.
- Show the existing no-safe-area warning immediately when a pawn is switched to **Flee to safe area** while no safe cells are painted.
- Add `DS_Log.WarningOnce` and `DS_Log.ErrorOnce` helpers for non-spamming diagnostics.
- Log safe-area alert initialization and activation to make future alert-registration problems visible in the RimWorld log.
- Synchronize the mod metadata and assembly version to `0.3.1`.

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
