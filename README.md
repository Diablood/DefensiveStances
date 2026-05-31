# Defensive Stances

A RimWorld 1.6 mod prototype that adds defensive doctrines alongside the vanilla hostility response.

## Naming

- Workshop display name: **Defensive Stances**
- Local mod folder: `DefensiveStances`
- GitHub repository: `Diablood/DefensiveStances`
- C# solution, project, assembly and root namespace: `DefensiveStances`
- Stable package ID: `diablood.defensivestances`

Do not change the package ID after a public release because it identifies the mod in load orders and saves.

## Prototype scope

The current 0.2.1 engineering scaffold contains:

- a custom `DefensiveBehaviorMode` enum without modifying RimWorld's vanilla enum;
- a saveable `GameComponent` with per-pawn doctrine state;
- two additional doctrines inside RimWorld's existing hostility-response dropdown;
- a dedicated map-level safe-area layer, independent from ordinary allowed areas;
- Architect tools to expand or clear safe-area cells;
- support for several disconnected shelters on the same map;
- temporary restriction to the global safe area during evacuation, with restoration after the danger has passed;
- an explicit pawn activity report while a colonist is moving toward shelter;
- direct aggressor recording when damage reaches a pawn in self-defense-only mode;
- correct melee retaliation for unarmed pawns in self-defense-only mode;
- an interception of `JobGiver_ConfigurableHostilityResponse.TryGiveJob` for the new doctrines;
- a migration path from the 0.1.x configured allowed-area prototype;
- English and French keyed translations.

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

## First in-game test for 0.2.1

1. Create or load a colony on RimWorld 1.6.
2. Open **Architect** → **Zone**.
3. Paint one or more shelters with **Expand safe area**. These cells may overlap stockpiles, growing zones and ordinary allowed areas.
4. Open the existing hostility-response dropdown for a colonist and choose **Flee to safe area**.
5. Spawn a hostile threat in dev mode and verify that the colonist reaches one of the painted shelters and remains restricted there temporarily.
6. Choose **Self-defense only** for another colonist, let a hostile pawn damage them and verify that retaliation targets the aggressor rather than an arbitrary nearby enemy.

See `docs/FUNCTIONAL_CHECKLIST.md` for the validation matrix.

## Repository layout

```text
About/                         RimWorld metadata
1.6/Assemblies/                compiled output
1.6/Languages/                 keyed translations
1.6/Patches/                   XML injection into Architect → Zone
Source/DefensiveStances/       C# project
Source/DefensiveStances.sln    Visual Studio solution
LoadFolders.xml                RimWorld version folder routing
```

## Known limitations

- The custom dropdown entries currently reuse vanilla icons.
- The map-level safe area is one global layer containing any number of disconnected shelters; named or prioritized shelter groups are not implemented yet.
- Self-defense currently starts after a damage event with a hostile instigator. Missed shots are not tracked yet.
- A short grace period is used before restoring the previous allowed area after evacuation.
- Combat Extended, multiplayer and large mod-list compatibility have not yet been tested.
