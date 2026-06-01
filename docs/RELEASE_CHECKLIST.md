# Defensive Stances release checklist

Use this checklist before publishing an alpha, beta or stable release.

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
- [ ] Confirm that `About/Preview.png` and `About/ModIcon.png` render correctly.

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
- [ ] Create an annotated Git tag such as `v0.9.0-alpha`.
- [ ] Push the commit and tag to GitHub.
- [ ] Create a GitHub pre-release and attach `dist/DefensiveStances-<version>.zip`.
- [ ] Review `docs/WORKSHOP_DESCRIPTION.md` before a Steam Workshop upload.
- [ ] Add Harmony as a required Workshop item when publishing on Steam.
