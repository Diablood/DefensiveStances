# Functional checklist for 0.4.0-dev

## Load and save

- [ ] The mod loads after Harmony with no red errors.
- [ ] The startup log reports `Version 0.4.0.0 loaded. Harmony patches applied.`
- [ ] The RimWorld log no longer warns that `DefensiveHostilityResponseUI` needs `StaticConstructorOnStartup`.
- [ ] A new game can be created.
- [ ] An existing 0.1.x save can be loaded with the mod enabled.
- [ ] Pawn doctrines persist after save and reload.
- [ ] Painted safe-area cells persist after save and reload.
- [ ] A legacy configured allowed area is copied into the dedicated safe-area layer when first used.

## Existing hostility-response dropdown

- [ ] No extra **Defensive stance** gizmo appears when selecting a pawn.
- [ ] No extra per-pawn **Safe area** gizmo appears when selecting a pawn.
- [ ] The existing hostility-response dropdown contains `Ignore`, `Attack`, `Flee`, `Flee to safe area` and `Self-defense only`.
- [ ] `Attack` and `Self-defense only` are absent for pawns incapable of violence.
- [ ] Dropdown painting across multiple pawn rows still works.
- [ ] Returning to a vanilla choice clears the custom doctrine cleanly.

## Dedicated global safe area

- [ ] **Architect** → **Zone** contains **Expand safe area** and **Clear safe area**.
- [ ] The safe-area overlay appears while either tool is active.
- [ ] Safe cells can overlap a growing zone.
- [ ] Safe cells can overlap a stockpile.
- [ ] Safe cells can overlap an ordinary allowed area.
- [ ] Several disconnected shelters can be painted on one map.
- [ ] Multiple maps keep separate safe-area layers.

## Flee to safe area

- [ ] A pawn in danger moves toward a reachable cell inside the global safe area.
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

## Alerts

- [ ] **No safe area configured** appears when at least one spawned free colonist uses **Flee to safe area** and their map contains no safe cells.
- [ ] Clicking the alert jumps to one of the affected colonists.
- [ ] The alert disappears after painting at least one safe cell on the affected map.
- [ ] The alert does not appear when no colonist uses **Flee to safe area**.
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

## Deferred translation audit

- [ ] Generate RimWorld's French translation report and attribute the warning observed during development.
- [ ] Verify every `DS_*` keyed translation in English and French before a stable release.
