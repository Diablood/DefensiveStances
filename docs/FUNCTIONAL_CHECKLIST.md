# Functional checklist for 1.0.2-dev

## Load and save

- [ ] The mod loads after Harmony with no red errors.
- [ ] The startup log reports `Version 1.0.2.0 loaded. Harmony patches applied.`
- [ ] The RimWorld log no longer warns that `DefensiveHostilityResponseUI` needs `StaticConstructorOnStartup`.
- [ ] A new game can be created.
- [ ] An existing 0.1.x save can be loaded with the mod enabled.
- [ ] Pawn doctrines persist after save and reload.
- [ ] Painted safe-area cells persist after save and reload.
- [ ] A map-wide emergency alarm remains active after save and reload, and its pawns are sheltered again after loading.
- [ ] A legacy configured allowed area is copied into the dedicated safe-area layer when first used.

## Existing hostility-response dropdown

- [ ] No extra **Defensive stance** gizmo appears when selecting a pawn.
- [ ] No extra per-pawn **Safe area** gizmo appears when selecting a pawn.
- [ ] The existing hostility-response dropdown contains `Ignore`, `Attack`, `Flee`, `Flee to safe area` and `Self-defense only`.
- [ ] `Attack` and `Self-defense only` are absent for pawns incapable of violence.
- [ ] Dropdown painting across multiple pawn rows still works.
- [ ] Returning to a vanilla choice clears the custom doctrine cleanly.

## Dedicated global safe area

- [ ] **Architect** → **Zone** contains **Expand safe area** and **Clear safe area**, without a view-only designator.
- [ ] **Expand safe area** uses the dedicated cyan shield-and-grid icon.
- [ ] **Clear safe area** uses the matching icon with a red diagonal removal ribbon.
- [ ] The bottom-right controls contain a dedicated safe-area visibility toggle.
- [ ] Enabling the toggle displays the shelter overlay without opening Architect or selecting a designator.
- [ ] Disabling the toggle hides the shelter overlay when no safe-area editing tool is active.
- [ ] With the safe-area toggle enabled, switching to the world view hides the shelter overlay completely.
- [ ] Panning the world map does not leave a screen-fixed shelter overlay behind.
- [ ] Returning from the world view to the local map displays the shelter overlay again when the toggle is still enabled.
- [ ] Switching to the world view also hides the shelter overlay while the global emergency alarm is active.
- [ ] The safe-area overlay still appears while either painting tool is active.
- [ ] Safe cells can overlap a growing zone.
- [ ] Safe cells can overlap a stockpile.
- [ ] Safe cells can overlap an ordinary allowed area.
- [ ] Several disconnected shelters can be painted on one map.
- [ ] Multiple maps keep separate safe-area layers.

## Flee to safe area

- [ ] A pawn in danger moves toward a reachable cell inside the global safe area.
- [ ] A pawn performing an ordinary automatic hauling or equipping job interrupts that job promptly when a nearby danger locally requires evacuation.
- [ ] A pawn performing an ordinary automatic job immediately heads toward shelter when a hostile ranged shot is directly aimed at them but misses.
- [ ] A pawn switched into **Flee to safe area** while a nearby danger is already active interrupts ordinary automatic work and heads toward shelter.
- [ ] Drafted pawns and explicit player-forced jobs are still not interrupted by the immediate-evacuation path.
- [ ] While moving toward shelter, the pawn activity report explicitly says that the pawn is fleeing to a safe area.
- [ ] A pawn chooses a reachable shelter when another painted shelter is inaccessible.
- [ ] The pawn's previous allowed area is restored after the grace period.
- [ ] A manual allowed-area change made during evacuation is not overwritten later.
- [ ] If no safe cell exists, vanilla flee behavior remains available as fallback.
- [ ] An empty or unreachable safe layer does not cause an exception.
- [ ] A pawn facing an empty safe layer receives a focused in-game warning and falls back to vanilla fleeing.
- [ ] A pawn facing an unreachable safe layer receives a focused in-game warning and falls back to vanilla fleeing.
- [ ] Failure warnings use the colored `[Defensive Stances]` prefix in the RimWorld log.
- [ ] Repeated evacuation failures are throttled instead of flooding messages every think cycle.
- [ ] A failed evacuation attempt does not leave the pawn restricted to an empty or unreachable safe-area layer.
- [ ] After reaching shelter, a pawn remains restricted to the safe-area layer while danger persists.
- [ ] After reaching shelter, a pawn remains restricted beyond the grace period while any active hostile threat still exists on the map.
- [ ] A distant active pirate does not trigger evacuation for a pawn who never had a local reason to flee.
- [ ] After danger clears, the previous allowed area is restored only after the grace period.
- [ ] If a non-forced automatic job carries an evacuated pawn outside shelter, the pawn is redirected to a reachable safe cell.
- [ ] If an automatic movement job would leave shelter, it is interrupted while evacuation remains active.
- [ ] Containment-recovery logs are throttled instead of flooding the RimWorld log.
- [ ] A player-forced priority job outside shelter temporarily wins over containment and is not cancelled.
- [ ] After a player-forced priority job ends outside shelter, containment resumes and redirects the pawn.
- [ ] Drafting temporarily wins over containment; after undrafting outside shelter, the pawn is redirected into the safe area.
- [ ] Clearing every safe cell during an active evacuation immediately interrupts an in-progress shelter `Goto`, restores the pawn's previous allowed area and emits the existing throttled warning.

## Global emergency evacuation

- [ ] The bottom-right controls contain a dedicated siren toggle next to the safe-area visibility control.
- [ ] Clicking the siren with no painted safe cell leaves the toggle disabled and displays a warning message.
- [ ] Clicking the siren with at least one painted safe cell enables the global alarm for the current map.
- [ ] Enabling the global alarm immediately interrupts ordinary and player-forced jobs for undrafted controllable pawns and sends them toward a reachable shelter.
- [ ] Individual pawn doctrines are temporarily ignored while the global alarm is active.
- [ ] Drafted pawns remain drafted and are not redirected by the global alarm.
- [ ] Undrafting a pawn while the global alarm remains active immediately sends that pawn toward a reachable shelter.
- [ ] A pawn that cannot reach any painted safe cell produces a clickable warning message targeting that pawn.
- [ ] Reachable pawns still evacuate when another pawn is unreachable.
- [ ] The safe-area overlay stays visible while the global alarm is active.
- [ ] Clicking the siren again lifts the alarm and restores previous allowed areas for pawns sheltered only by the global alarm.
- [ ] A pawn that still has an individual local-danger evacuation reason remains sheltered after the global alarm is lifted.
- [ ] Each map keeps an independent alarm state.
- [ ] Save and reload preserves the alarm state independently for each map.
- [ ] Clearing every safe cell while the alarm is active keeps the alarm enabled, restores unusable restrictions and shows the existing safe-area warnings.
- [ ] Painting a usable refuge again while the alarm remains active automatically resumes sheltering.

## Alerts

- [ ] **No safe area configured** appears when at least one spawned controllable pawn uses **Flee to safe area** and their map contains no safe cells.
- [ ] Clicking the alert jumps to one of the affected colonists.
- [ ] The alert disappears after painting at least one safe cell on the affected map.
- [ ] The alert does not appear when no colonist uses **Flee to safe area**.
- [ ] **Safe area unreachable** appears when painted safe cells exist but none is reachable for a configured colonist.
- [ ] Clicking **Safe area unreachable** jumps to one of the affected colonists.
- [ ] **Safe area unreachable** disappears after opening a path or painting a reachable safe cell.
- [ ] The empty-layer and unreachable-layer alerts do not report the same pawn at the same time.
- [ ] Multiple maps evaluate their safe-area layers independently.

## Self-defense only

- [ ] A pawn ignores nearby hostiles before being damaged.
- [ ] A pawn retaliates against the direct hostile instigator after damage.
- [ ] A pawn retaliates when a hostile ranged shot is directly aimed at them but misses.
- [ ] A pawn retaliates when a hostile ranged shot aimed at them strikes cover instead.
- [ ] A pawn retaliates when a hostile melee attack misses or is dodged.
- [ ] An unarmed pawn retaliates in melee without repeatedly starting a static shooting job.
- [ ] A pawn does not choose an arbitrary nearby enemy.
- [ ] A pawn incapable of violence does not retaliate.
- [ ] Retaliation expires after the aggressor is gone, non-hostile or stale.

## Regression passes

- [ ] Vanilla hostility responses still behave exactly as before.
- [ ] Drafted pawns remain under player control.
- [ ] Forced jobs are not replaced.

## Mod settings

- [ ] **Options** → **Mod settings** contains a **Defensive Stances** entry.
- [ ] The default restoration grace period is 10 seconds.
- [ ] The default containment check interval is 1 second.
- [ ] Changing the restoration grace period changes how long an evacuated pawn remains restricted after danger clears.
- [ ] Changing the containment interval changes how quickly an undrafted pawn outside shelter is redirected back into the safe area.
- [ ] Disabling transient safe-area warning messages hides focused in-game messages while persistent alerts and colored log warnings remain active.
- [ ] Disabling vanilla fleeing fallback prevents the vanilla flee job when the safe-area layer is empty or unreachable.
- [ ] Re-enabling vanilla fleeing fallback restores the previously validated fallback behavior.
- [ ] **Reset to defaults** restores 10 seconds, 1 second and both enabled checkboxes.
- [ ] Settings survive a full RimWorld restart.

## Translation audit

- [ ] Run `./tools/validate-translations.ps1` and confirm that all mod-owned keyed translations pass validation.
- [ ] Confirm that `build.ps1` runs the same validator before compilation.
- [x] Generate RimWorld's French `TranslationReport.txt` and attribute the warning observed during development to Core language data in the no-DLC test configuration.
- [ ] Use `./tools/find-translation-report.ps1` to locate the generated report before the stable release.

## Release packaging

- [ ] `About/About.xml` contains the intended `modVersion`, GitHub `url`, supported RimWorld version and Harmony dependency.
- [ ] `About/Preview.png` renders correctly in the RimWorld mod manager.
- [ ] `About/ModIcon.png` renders correctly where RimWorld displays the compact mod icon.
- [ ] `./tools/package-release.ps1 -SkipBuild` creates `dist/DefensiveStances-<version>.zip` after a successful local build.
- [ ] The distribution ZIP contains `About/`, `1.6/`, `LoadFolders.xml`, `LICENSE` and `README.md` under one top-level `DefensiveStances/` folder.
- [ ] The distribution ZIP excludes `Source/`, `tools/`, `docs/`, `.git/`, PDB files and intermediate build folders.
- [ ] Extracting the distribution ZIP into a temporary local `Mods` folder passes a final startup smoke test.

## Self-defense downed-target regression

- [ ] When the recorded aggressor is downed, the pawn stops retaliating automatically and resumes ordinary work without alternating attack jobs.
- [ ] If the downed aggressor later stands up, no automatic retaliation resumes until a new direct attack occurs.
