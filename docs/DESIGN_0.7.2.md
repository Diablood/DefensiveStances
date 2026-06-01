# Design notes for 0.7.2: immediate evacuation activation

## Problem

The existing think-tree interception can create the correct shelter job when RimWorld asks for a new hostility response. However, an ordinary automatic job such as hauling or equipping may continue until its current step finishes. A hostile ranged shot that misses does not call the vanilla damage-notification path, so a working pawn can pick up an item before the next think-tree evaluation redirects them.

## Rule

A pawn using **Flee to safe area** now starts evacuation through two complementary paths:

| Trigger | Reaction |
| --- | --- |
| RimWorld's local `SelfDefenseUtility.ShouldStartFleeing(pawn)` condition becomes true while the pawn has an ordinary automatic job | Poll at the configured containment interval and interrupt the automatic job to start sheltering. |
| A hostile ranged or melee attack directly targets the pawn | Reuse the direct-attack hooks and start sheltering immediately, including missed shots. |

Drafted pawns and explicit player-forced jobs still win over automation.

## Separation from evacuation lifetime

Version 0.7.1 remains responsible for deciding when an already-started evacuation may end. Version 0.7.2 only improves how quickly evacuation starts. The two rules intentionally remain separate:

- start from a pawn-local danger or a direct hostile attack;
- remain sheltered while any active map-level hostile threat persists;
- restore the previous allowed area only after the configured grace period.
