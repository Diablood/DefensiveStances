# Design notes for 0.7: mod settings

## Goal

Expose a small set of gameplay settings without changing the validated default behavior.

## Settings storage

`DefensiveStancesMod` derives from `Verse.Mod` and loads a single `DefensiveStancesSettings` instance through RimWorld's standard `GetSettings<T>()` path. The settings are stored independently from save-game data, so the same preferences apply to all colonies until changed by the player.

## Available settings

| Setting | Default | Range | Effect |
| --- | ---: | ---: | --- |
| Restore previous allowed area after danger clears | 10 s | 0–60 s | Controls the evacuation grace period. |
| Safe-area containment check interval | 1 s | 0.25–5 s | Controls how frequently active evacuations are rechecked. |
| Show in-game safe-area warning messages | Enabled | — | Controls transient focused messages. Persistent alerts and logs remain active. |
| Allow vanilla fleeing when no safe cell is usable | Enabled | — | Keeps the previously validated vanilla fallback optional. |

## Compatibility

The default values reproduce the behavior used in 0.6.1-dev. Existing saves do not need migration because settings are not stored inside `DefensiveStancesGameComponent`.
