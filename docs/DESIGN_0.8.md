# Design 0.8 — Global emergency evacuation

## Goal

Provide a map-wide emergency alarm that sends every undrafted controllable pawn to the dedicated safe-area layer, regardless of their individual hostility-response doctrine.

## User interface

A dedicated siren toggle is injected alongside the existing bottom-right map controls. The state belongs to the current map:

- inactive → clicking attempts to activate the emergency alarm;
- active → clicking lifts the emergency alarm;
- no painted safe cell → activation is rejected, the toggle stays off and a warning message is displayed.

The safe-area overlay remains visible while the alarm is active so the current refuge layer is easy to inspect.

## Persistence

`DefensiveStancesGameComponent` serializes one `GlobalEmergencyEvacuationMapState` per map. Each record stores:

```text
map reference
global alarm active flag
```

After loading a save, active alarms are enforced again for the corresponding maps.

## Pawn behavior

When the alarm is active:

1. every spawned controllable pawn is evaluated;
2. drafted pawns remain under direct player control;
3. undrafted pawns immediately receive a safe-area evacuation job when a reachable cell exists;
4. individual hostility-response doctrines and explicit priority jobs do not block the emergency return;
5. unreachable pawns receive the existing clickable focused warning;
6. undrafting a pawn triggers an immediate re-evaluation through a Harmony postfix on the `Pawn_DraftController.Drafted` setter.

## Interaction with doctrine-triggered evacuation

Pawn evacuation tracking now distinguishes two reasons:

```text
local danger doctrine
global emergency alarm
```

The same containment and restoration infrastructure is reused. Lifting the alarm removes only the global reason. A pawn that still has a local doctrine reason remains sheltered until that reason clears normally.

## Scope

The implementation scans `MapPawns.AllPawnsSpawned` and keeps pawns for which `Pawn.IsPlayerControlled` is true and `playerSettings` is available. Prisoners and ordinary animals are not included.
