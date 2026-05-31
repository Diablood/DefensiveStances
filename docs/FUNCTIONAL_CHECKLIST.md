# Functional checklist for 0.1.0-dev

## Load and save

- [ ] The mod loads after Harmony with no red errors.
- [ ] A new game can be created.
- [ ] An existing save can be loaded with the mod enabled.
- [ ] Pawn doctrines persist after save and reload.
- [ ] The configured safe area persists after save and reload.

## Vanilla doctrine

- [ ] A pawn left on vanilla doctrine behaves exactly as before.
- [ ] Existing vanilla hostility response controls remain usable.

## Flee to safe area

- [ ] The safe-area picker lists allowed areas only.
- [ ] A pawn in danger moves toward a reachable cell inside the configured safe area.
- [ ] The pawn's previous allowed area is restored after the grace period.
- [ ] A manual allowed-area change made during evacuation is not overwritten later.
- [ ] If no safe area exists, vanilla flee behavior remains available as fallback.
- [ ] An empty, deleted or unreachable safe area does not cause an exception.

## Self-defense only

- [ ] A pawn ignores nearby hostiles before being damaged.
- [ ] A pawn retaliates against the direct hostile instigator after damage.
- [ ] A pawn does not choose an arbitrary nearby enemy.
- [ ] A pawn incapable of violence does not retaliate.
- [ ] Retaliation expires after the aggressor is gone, non-hostile or stale.

## Regression passes

- [ ] Drafted pawns remain under player control.
- [ ] Forced jobs are not replaced.
- [ ] Pawns without configurable hostility responses receive no extra gizmos.
- [ ] Multiple maps keep separate safe-area settings.
