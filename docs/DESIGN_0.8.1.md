# Defensive Stances 0.8.1 design note

## Goal

Make the safe-area editing tools recognizable at a glance without changing their behavior. The previous prototype reused RimWorld's generic home-area icons, which did not communicate that the painted layer is a dedicated shelter overlay.

## Visual language

Both tools now use the same cyan shield-and-grid motif:

- **Expand safe area** uses the base shield icon.
- **Clear safe area** uses the same icon with a red diagonal removal ribbon, matching RimWorld's established remove-zone convention.

The icons are transparent 64×64 PNG textures stored under `1.6/Textures/UI/Designators/`.

## Scope

This revision changes only UI textures and their `ContentFinder<Texture2D>` paths. Safe-area painting, overlays, evacuation state and save compatibility remain unchanged.
