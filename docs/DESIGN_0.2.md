# 0.2 design note: dedicated safe-area layer

## Decision

Use one dedicated `Area_Safe` instance per map. It is a global layer that may contain any number of disconnected shelters.

## Why this is not a vanilla `Zone`

Vanilla zones share a single `ZoneManager` grid. A cell occupied by a growing zone or stockpile cannot simultaneously belong to another vanilla zone. A shelter must be able to overlap both, so the mod uses an `Area`-style boolean layer instead.

## Why this is not an ordinary allowed area

The safe layer is global and dedicated. It is not offered in the player's regular allowed-area picker. During an evacuation, the pawn is temporarily restricted to the safe layer, then their previous ordinary allowed area is restored.

## Future extension point

If separate shelter groups later need names, priorities or policies, the single layer can evolve into several `Area_Safe` instances. The first 0.2 implementation intentionally keeps the player workflow simple: paint safe cells globally and let fleeing pawns choose the nearest reachable shelter.
