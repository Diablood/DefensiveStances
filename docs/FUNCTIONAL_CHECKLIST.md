# Functional checklist for 0.2.1-dev

## Load and save

- [ ] The mod loads after Harmony with no red errors.
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

## Self-defense only

- [ ] A pawn ignores nearby hostiles before being damaged.
- [ ] A pawn retaliates against the direct hostile instigator after damage.
- [ ] An unarmed pawn retaliates in melee without repeatedly starting a static shooting job.
- [ ] A pawn does not choose an arbitrary nearby enemy.
- [ ] A pawn incapable of violence does not retaliate.
- [ ] Retaliation expires after the aggressor is gone, non-hostile or stale.

## Regression passes

- [ ] Vanilla hostility responses still behave exactly as before.
- [ ] Drafted pawns remain under player control.
- [ ] Forced jobs are not replaced.
