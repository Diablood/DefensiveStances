# Defensive Stances

**Defensive Stances** is a RimWorld 1.6 mod that adds defensive behavior options for player-controlled pawns without replacing the vanilla hostility responses.

Repository: `https://github.com/Diablood/DefensiveStances`

Current development hotfix: `1.0.3-dev`, based on the validated `1.0.0` stable release.

## Features

### Additional hostility responses

The existing RimWorld hostility-response dropdown receives two new choices:

- **Flee to safe area**: a pawn locally exposed to danger interrupts ordinary automatic work, reaches a dedicated safe-area cell and stays sheltered while hostile threats remain active on the map.
- **Self-defense only**: a pawn ignores nearby enemies until directly attacked, then retaliates against the aggressor. Direct ranged misses, shots absorbed by cover and missed or dodged melee attacks are recognized.

Vanilla **Ignore**, **Attack** and **Flee** remain unchanged.

### Dedicated global safe areas

Safe areas are a map-level overlay independent from stockpiles, growing zones and ordinary allowed areas. Several disconnected shelters may be painted on one map.

Open **Architect** → **Zone** and use:

- **Expand safe area**
- **Clear safe area**

The tools use dedicated shield-based icons. A bottom-right toggle displays or hides the safe-area overlay without opening an editing tool.

### Emergency evacuation alarm

A siren toggle in the bottom-right controls starts or stops a persistent map-wide emergency evacuation.

When enabled:

- every undrafted controllable pawn on the active map immediately heads toward a reachable safe cell;
- individual hostility-response choices are temporarily ignored;
- drafted pawns remain under direct player control;
- an undrafted pawn is sent to shelter immediately while the alarm remains active;
- inaccessible pawns produce clickable warning messages;
- the alarm state is saved independently for each map.

Trying to enable the alarm without painted safe cells leaves the toggle disabled and displays a warning.

### Configurable behavior

Open **Options** → **Mod settings** → **Defensive Stances** to adjust:

- the grace period before a previous allowed area is restored after danger clears;
- the interval used to check containment inside safe areas;
- transient safe-area warning messages;
- vanilla fleeing fallback when no usable safe cell exists.

The default settings reproduce the behavior validated during development.

## Requirements

- RimWorld `1.6`
- Harmony (`brrainz.harmony`)
- .NET SDK capable of targeting .NET Framework `4.7.2` for local compilation

The project references `Lib.Harmony.Ref` from NuGet and deliberately does **not** copy `0Harmony.dll` into the mod folder.

## Installation

### GitHub release archive

1. Download the release archive.
2. Extract the `DefensiveStances` folder into RimWorld's local `Mods` directory.
3. Enable Harmony.
4. Enable Defensive Stances after Harmony.

### Development checkout

Clone or copy the repository into RimWorld's local `Mods/DefensiveStances` directory, compile locally and enable the mod after Harmony.

## Build from Cursor

Open the repository folder in Cursor, then use the integrated terminal.

PowerShell:

```powershell
./build.ps1 -RimWorldManagedDir "D:\SteamLibrary\steamapps\common\RimWorld\RimWorldWin64_Data\Managed"
```

Bash:

```bash
./build.sh "$HOME/.steam/steam/steamapps/common/RimWorld/RimWorldLinux_Data/Managed"
```

The assembly is written directly to:

```text
1.6/Assemblies/DefensiveStances.dll
```

The Windows build script validates the English and French keyed translations before compilation.

## Create a distribution archive

After a successful local build, create a clean release ZIP without source code, development tools or documentation notes:

```powershell
./tools/package-release.ps1 `
    -RimWorldManagedDir "D:\SteamLibrary\steamapps\common\RimWorld\RimWorldWin64_Data\Managed"
```

To package an assembly that was already compiled:

```powershell
./tools/package-release.ps1 -SkipBuild
```

The resulting archive is written to:

```text
dist/DefensiveStances-<version>.zip
```

On Linux or macOS:

```bash
./tools/package-release.sh "$HOME/.steam/steam/steamapps/common/RimWorld/RimWorldLinux_Data/Managed"
```

## Distribution contents

The generated release archive intentionally contains only runtime and user-facing files:

```text
DefensiveStances/
├── About/
├── 1.6/
│   ├── Assemblies/DefensiveStances.dll
│   ├── Languages/
│   ├── Patches/
│   └── Textures/
├── LoadFolders.xml
├── LICENSE
└── README.md
```

## Translation validation

Run the repository validator manually with:

```powershell
./tools/validate-translations.ps1
```

After generating RimWorld's native `TranslationReport.txt`, locate it with:

```powershell
./tools/find-translation-report.ps1
```

The mod-owned keyed translations pass validation. The remaining French no-DLC warnings observed during development were attributed to Core language data.

## Compatibility notes

- RimWorld `1.6` is the only supported version at this stage.
- Harmony is required.
- Combat Extended, multiplayer and large mod-list compatibility still need dedicated testing.
- Preserve the project-specific `About/ModIcon.png` demon icon when preparing updates or release archives.
- Prisoners and ordinary animals are not included in the global emergency evacuation.
- Safe areas are one global layer per map. Named or prioritized shelter groups are not implemented yet.
- Self-defense tracks direct standard ranged and melee attacks. Near misses aimed at another pawn, indirect area attacks without an identifiable hostile instigator and attacks against nearby allies are outside the current scope.

## Repository layout

```text
About/                         RimWorld metadata, preview image and custom demon icon
1.6/Assemblies/                compiled output
1.6/Languages/                 English and French keyed translations
1.6/Patches/                   Architect → Zone injection
1.6/Textures/                  alarm and safe-area UI textures
Source/DefensiveStances/       C# project
tools/                         build-adjacent validation and packaging helpers
docs/                          design notes, release notes and test checklists
```

## Development documentation

- Full regression matrix: `docs/FUNCTIONAL_CHECKLIST.md`
- Release checklist: `docs/RELEASE_CHECKLIST.md`
- Workshop description draft: `docs/WORKSHOP_DESCRIPTION.md`
- Translation audit: `docs/TRANSLATION_AUDIT.md`

## License

See `LICENSE`.
