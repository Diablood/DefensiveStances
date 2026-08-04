Defensive Stances 1.0.6 docs/package updater r4

Use this after r3 stopped at docs/RELEASE_CHECKLIST.md.

From the repository root:

    Expand-Archive -Path .\DefensiveStances-1.0.6-dev-docs-and-package-copy-r4.zip -DestinationPath . -Force
    powershell -ExecutionPolicy Bypass -File .\finish-1.0.6-docs-package-r4.ps1
    git diff --check
    git status --short

Then build/package:

    powershell -ExecutionPolicy Bypass -File .\build.ps1 -RimWorldManagedDir "D:\SteamLibrary\steamapps\common\RimWorld\RimWorldWin64_Data\Managed"

    .\tools\package-release.ps1 `
        -SkipBuild `
        -CopyToMods `
        -RimWorldModsDir "D:\SteamLibrary\steamapps\common\RimWorld\Mods"
