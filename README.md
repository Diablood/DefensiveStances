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

The current 0.6.0 engineering scaffold contains:

- a custom `DefensiveBehaviorMode` enum without modifying RimWorld's vanilla enum;
- a saveable `GameComponent` with per-pawn doctrine state;
- two additional doctrines inside RimWorld's existing hostility-response dropdown;
- a dedicated map-level safe-area layer, independent from ordinary allowed areas;
- Architect tools to expand or clear safe-area cells;
- a bottom-right toggle that displays or hides the safe-area overlay without entering an editing tool;
- support for several disconnected shelters on the same map;
- active containment inside the global safe area during evacuation, with restoration after the danger has remained absent for a short grace period;
- automatic redirection back into shelter if a non-forced job carries an evacuated pawn outside the safe-area layer;
- temporary precedence for drafted control and player-forced orders, followed by automatic containment recovery;
- an explicit pawn activity report while a colonist is moving toward shelter;
- direct aggressor recording when a hostile ranged or melee attack is aimed at a pawn in self-defense-only mode, including missed shots and melee dodges;
- hostile-damage recording as a fallback for other direct damage sources;
- correct melee retaliation for unarmed pawns in self-defense-only mode;
- an interception of `JobGiver_ConfigurableHostilityResponse.TryGiveJob` for the new doctrines;
- a migration path from the 0.1.x configured allowed-area prototype;
- English and French keyed translations;
- a pre-build PowerShell validator that checks translation XML, duplicate keys, placeholders and English/French key parity;
- a centralized `DS_Log` wrapper with a colored mod-name prefix for diagnostics;
- startup diagnostics that include the loaded assembly version;
- main-thread initialization for the hostility-response UI textures;
- a persistent alert when at least one colonist expects a safe area but none has been painted on their map;
- a second persistent alert when painted shelter cells exist but none is reachable for a configured colonist;
- throttled in-game warnings and prefixed log entries when no safe cell is configured or reachable;
- safe fallback to vanilla fleeing without leaving a pawn restricted to an unusable safe-area layer.

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

## First in-game test for 0.6.0

1. Create or load a colony on RimWorld 1.6.
2. Open **Architect** → **Zone**.
3. Use the bottom-right safe-area visibility toggle and verify that the shelter overlay can be displayed or hidden without opening an editing tool.
4. Paint one or more shelters with **Expand safe area**. These cells may overlap stockpiles, growing zones and ordinary allowed areas.
5. Open the existing hostility-response dropdown for a colonist and choose **Flee to safe area**.
6. Spawn a hostile threat in dev mode and verify that the colonist reaches one of the painted shelters and remains restricted there until the danger has cleared.
7. Choose **Self-defense only** for another colonist, let a hostile pawn damage them and verify that retaliation targets the aggressor rather than an arbitrary nearby enemy.
8. Clear every safe cell while a colonist still uses **Flee to safe area** and verify that the **No safe area configured** alert appears.
9. Paint a shelter behind an inaccessible wall and verify that the **Safe area unreachable** alert appears for a configured colonist outside it.
10. Trigger danger with an empty or unreachable safe layer and verify the warning message, the prefixed log entry and the vanilla flee fallback.
11. Put a colonist in **Self-defense only**, fire directly at them with a deliberately inaccurate ranged attacker and verify that the colonist retaliates even when the shot misses.
12. Let a melee attacker miss or be dodged and verify that the colonist still retaliates against that attacker.
13. While evacuation remains active, force a non-player automatic job to carry a sheltered colonist outside the safe layer and verify that the mod redirects them into shelter.
14. Give the evacuated colonist a manual order outside the safe layer and verify that direct player control wins temporarily; after the forced order ends, verify that containment resumes.
15. Draft and undraft an evacuated colonist outside the refuge and verify that the mod does not override drafted control, then returns the undrafted pawn to shelter.

See `docs/FUNCTIONAL_CHECKLIST.md` for the validation matrix.

## Translation validation

The Windows build script validates the mod-owned keyed translations before compiling. The Bash build script runs the same validator when PowerShell Core (`pwsh`) is available:

```powershell
./tools/validate-translations.ps1
```

The validator checks that both XML files parse correctly, that no key is duplicated or left empty, and that French contains exactly the same `DS_*` keys as English. It does not replace RimWorld's own translation report, which also checks Core and other loaded content.

After generating RimWorld's `TranslationReport.txt` from the developer tools, locate it with:

```powershell
./tools/find-translation-report.ps1
```

## Repository layout

```text
About/                         RimWorld metadata
1.6/Assemblies/                compiled output
1.6/Languages/                 keyed translations
1.6/Patches/                   XML injection into Architect → Zone
Source/DefensiveStances/       C# project
Source/DefensiveStances.sln    Visual Studio solution
LoadFolders.xml                RimWorld version folder routing
tools/                         translation validation and report helpers
docs/                          design notes, audit notes and test checklist
```

## Known limitations

- The custom dropdown entries and the safe-area visibility toggle currently reuse vanilla icons.
- The map-level safe area is one global layer containing any number of disconnected shelters; named or prioritized shelter groups are not implemented yet.
- Self-defense handles standard direct ranged and melee attacks. Near misses aimed at another pawn, indirect area attacks without a hostile instigator and attacks against nearby allies are not tracked yet.
- A short grace period is used before restoring the previous allowed area after evacuation.
- Automatic containment deliberately yields to drafted control and explicit player-forced orders, then resumes afterward.
- Combat Extended, multiplayer and large mod-list compatibility have not yet been tested.
- The mod-owned keyed translations pass the repository validator. RimWorld's remaining French translation-report warnings were attributed to Core language data in the no-DLC test configuration.
