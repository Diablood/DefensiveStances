# Design notes for 0.7.1: evacuation threat lifetime

## Problem

The initial evacuation trigger uses RimWorld's local `SelfDefenseUtility.ShouldStartFleeing(pawn)` check. This is correct for deciding whether a pawn needs to seek shelter, but it is too narrow for deciding when a sheltered pawn may leave. Once the pawn reaches a protected room, the same pirate may be outside the local search radius and the restoration grace period can expire while the raid is still active.

## Rule

Starting and ending evacuation intentionally use different thresholds:

| Phase | Rule |
| --- | --- |
| Start evacuation | The pawn locally needs to flee according to `SelfDefenseUtility.ShouldStartFleeing(pawn)`. |
| Keep evacuation active | The pawn locally needs to flee **or** `GenHostility.AnyHostileActiveThreatToPlayer(map)` reports an active map-level threat. |
| Start restoration grace period | Neither local flee conditions nor map-level active threats remain. |

This keeps an evacuated pawn sheltered during an ongoing raid without automatically sending every configured colonist to shelter as soon as a distant hostile enters the map.
