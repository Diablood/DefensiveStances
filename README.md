# Defensive Stances

A RimWorld 1.6 mod prototype that adds defensive doctrines alongside the vanilla hostility response.

## Naming

- Workshop display name: **Defensive Stances**
- Local mod folder: `DefensiveStances`
- Suggested GitHub repository: `RimWorld-DefensiveStances`
- C# solution, project, assembly and root namespace: `DefensiveStances`
- Temporary package ID: `todoauthor.defensivestances`

Replace `todoauthor` once with your final author or GitHub handle before publishing. Do not change the package ID after a public release because it identifies the mod in load orders and saves.

## Prototype scope

The current scaffold is intentionally small but already contains the complete extension points for the first iteration:

- a custom `DefensiveBehaviorMode` enum without modifying RimWorld's vanilla enum;
- a saveable `GameComponent` with per-pawn and per-map state;
- a pawn gizmo that cycles between vanilla behavior, flee-to-safe-area and self-defense-only;
- a pawn gizmo to select a safe allowed area for the current map;
- temporary allowed-area restriction during evacuation, with restoration after the danger has passed;
- direct aggressor recording when damage reaches a pawn in self-defense-only mode;
- an interception of `JobGiver_ConfigurableHostilityResponse.TryGiveJob` for the new doctrines;
- English and French keyed translations.

This is a first playable engineering scaffold. It still needs in-game validation and balancing against an installed RimWorld 1.6 build.

## Requirements

- RimWorld 1.6
- Harmony (`brrainz.harmony`)
- .NET SDK capable of targeting .NET Framework 4.7.2

The project references `Lib.Harmony.Ref` from NuGet and deliberately does **not** copy `0Harmony.dll` into the mod folder.

## Build

The default Windows location is used when `RimWorldManagedDir` is not set:

```text
C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed
```

PowerShell:

```powershell
./build.ps1 -RimWorldManagedDir "D:\SteamLibrary\steamapps\common\RimWorld\RimWorldWin64_Data\Managed"
```

Bash:

```bash
./build.sh "$HOME/.steam/steam/steamapps/common/RimWorld/RimWorldLinux_Data/Managed"
```

The resulting assembly is written directly to:

```text
1.6/Assemblies/DefensiveStances.dll
```

## Install locally

Place the entire `DefensiveStances` directory inside RimWorld's `Mods` directory, enable Harmony before Core as requested by Harmony, then enable Defensive Stances after Harmony.

## First in-game test

1. Create or load a colony on RimWorld 1.6.
2. Paint an allowed area that represents a shelter.
3. Select a colonist and use **Defensive stance** to choose **Flee to safe area**.
4. Use **Safe area** to assign the shelter.
5. Spawn a hostile threat in dev mode and verify that the colonist moves into the assigned area and remains restricted there temporarily.
6. Switch another colonist to **Self-defense only**, let a hostile pawn damage them and verify that retaliation targets the aggressor rather than an arbitrary nearby enemy.

See `docs/FUNCTIONAL_CHECKLIST.md` for the validation matrix.

## Repository layout

```text
About/                         RimWorld metadata
1.6/Assemblies/                compiled output
1.6/Languages/                 keyed translations
Source/DefensiveStances/       C# project
Source/DefensiveStances.sln    Visual Studio solution
LoadFolders.xml                RimWorld version folder routing
```

## Known limitations of this scaffold

- No custom icons yet: built-in placeholder textures are used.
- The safe area is configured per map, not per colonist.
- Self-defense currently starts after a damage event with a hostile instigator. Missed shots are not tracked yet.
- A short grace period is used before restoring the previous allowed area after evacuation.
- Combat Extended, multiplayer and large mod-list compatibility have not yet been tested.
