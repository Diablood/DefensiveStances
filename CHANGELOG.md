# Changelog

## 1.0.3-dev

- Clear remembered **Self-defense only** aggression as soon as the player drafts the pawn.
- Treat drafting as a manual takeover that closes the current self-defense incident.
- Prevent an undrafted pawn from returning to combat against an enemy that stopped pursuing it unless a new direct attack occurs.
- Keep global-emergency undrafting behavior unchanged.

## 1.0.2-dev

- Stop **Self-defense only** retaliation as soon as the recorded pawn aggressor is downed or dead.
- Clear the remembered aggressor before creating another automatic attack job, preventing pawns from alternating between hauling and attacking an incapacitated enemy.
- Require a new direct hostile attack before retaliation can resume if a previously downed aggressor later stands up.
- Keep building and non-pawn aggressors supported while leaving evacuation behavior unchanged.

## 1.0.1-dev

- Hide the dedicated safe-area overlay while RimWorld is displaying the world view.
- Mirror RimWorld's `WorldRendererUtility.DrawingMap` guard before calling `Area.MarkForDraw()`.
- Prevent a screen-fixed shelter overlay from remaining visible while panning the world map.
- Keep the validated map overlay toggle and emergency-alarm behavior unchanged when returning to the local map view.

## 1.0.0-rc1

- Prepare the first stable release candidate without changing gameplay behavior.
- Synchronize RimWorld metadata and assembly versions to `1.0.0`.
- Preserve the project-specific red-and-black demon icon in `About/ModIcon.png`.
- Finalize the stable release checklist, Workshop description and release notes.
- Keep the validated English/French translations, packaging scripts and runtime-only distribution layout unchanged.

## 0.9.0-dev

- Prepare the repository for a first public alpha without changing gameplay behavior.
- Add the GitHub repository URL and a concise release-facing description to `About/About.xml`.
- Add `About/Preview.png` and `About/ModIcon.png` for mod-manager and Workshop presentation.
- Add `tools/package-release.ps1` and `tools/package-release.sh` to create a clean runtime-only distribution ZIP after a successful local build.
- Validate the compiled DLL file version against `About.xml` before Windows packaging to avoid stale release archives.
- Add a release checklist, a Workshop description draft and the 0.9 packaging design note.
- Ignore generated `dist/` archives in Git and synchronize metadata and assembly versions to `0.9.0`.

## 0.8.1-dev

- Replace the vanilla home-area textures used by **Expand safe area** and **Clear safe area** with dedicated cyan shield-and-grid icons.
- Keep both Architect tools visually related while adding a red diagonal removal ribbon to the clear action, matching RimWorld's vanilla remove-zone language.
- Add the two transparent 64×64 UI textures under `1.6/Textures/UI/Designators/`.
- Keep gameplay behavior unchanged while synchronizing the mod metadata and assembly version to `0.8.1`.

## 0.8.0-dev

- Add a dedicated bottom-right siren toggle for map-wide emergency evacuation.
- Persist the global alarm state independently for each map through the existing saveable game component.
- Immediately send every undrafted controllable pawn on the active map toward a reachable safe cell, regardless of their individual hostility-response doctrine.
- Keep drafted pawns under direct player control; when a drafted pawn is released while the alarm remains active, trigger sheltering immediately.
- Reject alarm activation when the map contains no painted safe cell, keep the toggle disabled and show a focused warning message.
- Reuse clickable pawn-targeted warnings and persistent alerts for colonists who cannot reach any safe cell while the global alarm is active.
- Keep the safe-area overlay visible while the emergency alarm is active and add a dedicated siren texture.
- Preserve legacy doctrine-triggered evacuations and migrate pre-0.8 active evacuation tracking to the local-danger reason.
- Synchronize the mod metadata and assembly version to `0.8.0`.

## 0.7.3-dev

- Add the missing `DefensiveStances.Components` and `DefensiveStances.Settings` imports to `DefensiveEvacuationUtility`.
- Restore compilation of the immediate-evacuation path introduced in `0.7.2-dev`.
- Keep gameplay behavior unchanged while synchronizing the mod metadata and assembly version to `0.7.3`.

## 0.7.2-dev

- Interrupt non-forced automatic jobs immediately when a pawn using **Flee to safe area** is directly targeted by a hostile ranged or melee attack.
- Reuse the direct-attack hooks introduced for self-defense mode so a missed shot can trigger sheltering before the pawn finishes hauling or equipping an item.
- Poll inactive **Flee to safe area** pawns at the configured containment interval and start evacuation when RimWorld's local flee condition becomes true, even while an ordinary automatic job is running.
- Start the same local-danger check immediately when the player switches a pawn into **Flee to safe area** mode.
- Preserve direct player control: drafted pawns and player-forced jobs are not interrupted.
- Synchronize the mod metadata and assembly version to `0.7.2`.

## 0.7.1-dev

- Keep an active evacuation running while RimWorld still reports any active hostile threat on the pawn's map.
- Start the restoration grace period only after both the pawn-local flee condition and map-level hostile-threat check have cleared.
- Preserve the existing behavior where evacuation starts only for pawns that locally need to flee; a distant raid does not automatically send every configured colonist to shelter.
- Add a dedicated threat-lifetime utility, update the settings tooltip and extend the regression checklist.
- Synchronize the mod metadata and assembly version to `0.7.1`.

## 0.7.0-dev

- Add a standard RimWorld mod-settings screen through `DefensiveStancesMod` and `DefensiveStancesSettings`.
- Make the post-danger restoration grace period configurable from 0 to 60 seconds while preserving the previous 10-second default.
- Make the active containment check interval configurable from 0.25 to 5 seconds while preserving the previous 1-second default.
- Allow players to hide transient safe-area warning messages while keeping persistent alerts and colored diagnostic logs active.
- Make vanilla fleeing fallback optional when the configured safe-area layer is empty or unreachable.
- Add a reset-to-defaults action and English/French labels for every setting.
- Add the 0.7 design note and extend the validation checklist.
- Synchronize the mod metadata and assembly version to `0.7.0`.

## 0.6.1-dev

- Notify active evacuations immediately whenever safe-area editing adds or removes a shelter cell.
- Cancel an in-progress mod-issued shelter `Goto` as soon as the active refuge layer becomes empty or unreachable.
- Restore the pawn's previous allowed area and reuse the existing throttled warning path without waiting for the periodic containment tick.
- Clarify the player-precedence checklist: vanilla has no direct undrafted movement order, so use a forced-priority job or drafted movement for validation.
- Synchronize the mod metadata and assembly version to `0.6.1`.

## 0.6.0-dev

- Keep evacuated pawns restricted to the global safe-area layer until danger has remained absent for the existing grace period.
- Recheck active evacuations every 60 ticks and redirect pawns back to a reachable shelter cell if an automatic job moves them outside the refuge.
- Interrupt non-forced automatic movement that would carry a sheltered pawn out of the active safe-area layer.
- Preserve direct player control: drafted pawns and player-forced orders temporarily bypass automatic containment, which resumes afterward.
- Restore the previous allowed area and reuse the existing throttled warning when an active shelter layer becomes empty or unreachable.
- Add throttled containment-recovery diagnostics through the colored `DS_Log` wrapper.
- Add the 0.6 design note and extend the validation checklist.
- Synchronize the mod metadata and assembly version to `0.6.0`.

## 0.5.1-dev

- Remove the view-only **View safe area** designator from **Architect** → **Zone**.
- Add a bottom-right safe-area visibility toggle alongside RimWorld's existing map overlay controls.
- Keep the shelter layer visible while the toggle is enabled without opening Architect or exposing designator drawing modes.
- Retain only **Expand safe area** and **Clear safe area** as shelter editing tools.
- Synchronize the mod metadata and assembly version to `0.5.1`.

## 0.5.0-dev

- Add **View safe area** to **Architect** → **Zone** so the shelter overlay can be inspected without painting or clearing cells.
- Add a persistent **Safe area unreachable** alert for colonists using **Flee to safe area** when painted shelter cells exist but none can be reached from their current position.
- Keep the empty-layer and unreachable-layer alerts mutually exclusive so each configuration problem has one clear explanation.
- Reuse a shared nearest-reachable-safe-cell lookup for runtime evacuation and proactive alert checks.
- Record the completed French translation-report attribution: remaining no-DLC warnings belong to Core language data, not Defensive Stances.
- Add the 0.5 design note and extend the validation checklist.
- Synchronize the mod metadata and assembly version to `0.5.0`.

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

- Add a persistent **No safe area configured** alert when a spawned controllable pawn uses **Flee to safe area** but their map has no painted safe cells.
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
