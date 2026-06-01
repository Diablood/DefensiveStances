# 0.5 design note: shelter visibility and proactive reachability feedback

## Goal

Make the dedicated safe-area layer easier to inspect before a threat appears and report unusable shelter layouts before evacuation is needed.

## View-only designator

`Designator_AreaSafeView` is injected into **Architect** → **Zone** alongside the existing expand and clear tools. While selected, it draws the global safe-area layer but never changes cells. This provides a safe inspection mode without requiring the player to paint or erase tiles.

## Reachability alert

`Alert_NoReachableSafeArea` reports spawned free colonists configured with **Flee to safe area** when their map contains painted safe cells but none can be reached from their current position.

The empty-layer case remains owned by `Alert_NoSafeAreaConfigured`, so the two persistent alerts do not report the same configuration problem at the same time.

## Shared destination lookup

The nearest reachable-cell search now lives in `DefensiveSafeAreaUtility.TryFindReachableSafeCell`. Runtime evacuation and proactive alerts use the same rule set:

- the pawn must be spawned on the same map as the safe-area layer;
- shelter cells must be standable;
- a valid path may cross deadly danger, matching emergency evacuation semantics;
- the nearest reachable cell is preferred.

## Performance boundary

The reachability alert intentionally runs through RimWorld's alert evaluation cycle rather than every game tick. It is appropriate for the prototype and keeps the path checks outside the regular evacuation-maintenance tick. If large colonies or unusually large shelter layers reveal a measurable cost, the result can be cached per pawn and invalidated when the safe-area layer or reachability topology changes.
