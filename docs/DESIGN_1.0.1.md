# Defensive Stances 1.0.1 overlay hotfix

## Problem

The safe-area overlay was refreshed from a Harmony postfix on `MapInterface.MapInterfaceUpdate()`. RimWorld returns early from the original method when the local map renderer is not active, but Harmony postfixes still run. The mod therefore continued to call `Area.MarkForDraw()` while the player was viewing the world map.

The stale map mesh appeared as a screen-fixed overlay when panning the world view.

## Fix

`DefensiveSafeAreaOverlay.DrawIfVisible()` now mirrors RimWorld's local-map rendering guard before refreshing the area mesh:

```csharp
if (!WorldRendererUtility.DrawingMap)
{
    return;
}
```

The method also returns when `Find.CurrentMap` is null.

## Compatibility

The change affects drawing only. It does not modify saved data, safe-area painting, individual doctrines, emergency evacuation state or containment behavior.
