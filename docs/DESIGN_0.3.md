# 0.3 design note: evacuation feedback and safe fallback

## Goal

Make invalid shelter configurations visible to the player without changing the intended doctrine semantics.

## Static feedback

`Alert_NoSafeAreaConfigured` reports maps where at least one spawned free colonist uses `FleeToSafeArea` while the dedicated safe-area layer is empty. The alert points to the affected colonists and disappears as soon as at least one cell is painted.

## Runtime feedback

An evacuation can still fail even when cells are painted, for example when every shelter is cut off by walls or forbidden terrain. `DefensiveEvacuationFeedback` emits a focused in-game warning and a prefixed log entry for the affected pawn. Feedback is throttled per pawn and failure reason to prevent spam from repeated think-tree evaluations.

## Safe fallback

Reachability is validated before the pawn is temporarily restricted to the safe-area layer. If an evacuation was already active and its shelter becomes unusable, the previous allowed area is restored before the vanilla flee fallback is allowed to run.
