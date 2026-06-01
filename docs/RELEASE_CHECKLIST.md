# Defensive Stances release checklist

Use this checklist before publishing a release candidate or stable release.

## Version metadata

- [ ] Update `About/About.xml` → `modVersion`.
- [ ] Update `Source/DefensiveStances/Properties/AssemblyInfo.cs` → `AssemblyVersion` and `AssemblyFileVersion`.
- [ ] Add an entry to `CHANGELOG.md`.
- [ ] Confirm that the Git commit message starts with the intended version.

## Repository validation

- [ ] Run `./tools/validate-translations.ps1`.
- [ ] Confirm that English and French keys match exactly.
- [ ] Parse every XML file successfully.
- [ ] Confirm that `.editorconfig` formatting remains at four spaces where applicable.
- [ ] Confirm that `About/Preview.png` renders correctly.
- [ ] Confirm that `About/ModIcon.png` is the project-specific red-and-black demon icon and has not been replaced.

## Clean local build

From Cursor's integrated terminal:

```powershell
Remove-Item ./1.6/Assemblies/DefensiveStances.dll -Force -ErrorAction SilentlyContinue

./build.ps1 `
    -RimWorldManagedDir "D:\SteamLibrary\steamapps\common\RimWorld\RimWorldWin64_Data\Managed"
```

- [ ] Confirm that the build succeeds.
- [ ] Confirm that the DLL exists in `1.6/Assemblies/`.
- [ ] Confirm that the assembly file version matches `About.xml`.

## In-game smoke test

Use a minimal active mod list:

```text
Harmony
Core
Defensive Stances
```

- [ ] Start RimWorld without a red error related to Defensive Stances.
- [ ] Confirm the colored startup log contains the intended DLL version.
- [ ] Confirm that the hostility-response dropdown contains the two added doctrines.
- [ ] Paint and clear safe-area cells using the dedicated icons.
- [ ] Toggle safe-area visibility.
- [ ] Test doctrine-triggered sheltering with a nearby hostile pawn.
- [ ] Test self-defense after a direct missed ranged shot.
- [ ] Activate and deactivate the global siren.
- [ ] Save and reload while the global alarm is active.
- [ ] Confirm that a drafted pawn remains under direct control during the global alarm.

Use `docs/FUNCTIONAL_CHECKLIST.md` for a full regression pass before a stable release.

## Build the distribution ZIP

```powershell
./tools/package-release.ps1 -SkipBuild
```

- [ ] Confirm that `dist/DefensiveStances-<version>.zip` exists.
- [ ] Open the ZIP and confirm that it contains one top-level `DefensiveStances/` folder.
- [ ] Confirm that `1.6/Assemblies/DefensiveStances.dll` is present.
- [ ] Confirm that `Source/`, `tools/`, `docs/`, `.git/` and build artifacts are absent.
- [ ] Extract the ZIP into a temporary RimWorld `Mods` folder and perform one final startup smoke test.

## Publish

- [ ] Commit the validated source tree.
- [ ] For the candidate, create an annotated Git tag such as `v1.0.0-rc1`.
- [ ] After final validation, create the stable annotated tag `v1.0.0`.
- [ ] Push the commit and intended tag to GitHub.
- [ ] Attach `dist/DefensiveStances-<version>.zip` to the matching GitHub release. Use a pre-release for the RC and a normal release for `v1.0.0`.
- [ ] Review `docs/WORKSHOP_DESCRIPTION.md` before a Steam Workshop upload.
- [ ] Add Harmony as a required Workshop item when publishing on Steam.
