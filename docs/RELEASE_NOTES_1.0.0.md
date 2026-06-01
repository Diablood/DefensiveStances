# Defensive Stances 1.0.0 release notes

## Scope

`1.0.0-rc1` is a release candidate for the first stable version of Defensive Stances. It deliberately introduces no new gameplay behavior after the validated `0.9.0` build.

## Included features

- Two additional hostility responses in the vanilla pawn dropdown: **Flee to safe area** and **Self-defense only**.
- Dedicated map-level safe-area overlay that can overlap stockpiles, growing zones and ordinary allowed areas.
- Dedicated safe-area paint and clear tools with shield-based icons.
- Bottom-right safe-area visibility toggle.
- Persistent bottom-right global emergency evacuation siren.
- Clickable warnings for missing or unreachable shelters.
- Configurable shelter restoration delay, containment interval, transient warnings and vanilla flee fallback.
- English and French keyed translations.
- Colored startup and diagnostic logs.

## Candidate validation

Before creating the stable `v1.0.0` tag:

1. Compile locally against RimWorld 1.6.
2. Run the short in-game smoke test from `docs/RELEASE_CHECKLIST.md`.
3. Generate `dist/DefensiveStances-1.0.0.zip`.
4. Extract that ZIP into a temporary Mods directory and perform a final startup check.
5. Verify that the custom red-and-black demon icon is present in `About/ModIcon.png`.
