# Translation audit

## Scope

This pass validates the translation files owned by Defensive Stances:

```text
1.6/Languages/English/Keyed/DefensiveStances.xml
1.6/Languages/French/Keyed/DefensiveStances.xml
```

The repository validator checks:

- XML parsing;
- the `<LanguageData>` root element;
- duplicate keyed entries;
- empty values;
- `TODO` placeholders;
- exact key parity between English and French.

Run it from Cursor's integrated PowerShell terminal:

```powershell
./tools/validate-translations.ps1
```

The Windows build script runs the same validation automatically before `dotnet build`. The Bash build script also runs it when PowerShell Core (`pwsh`) is available.

## Current result

The translation files contain matching English and French `DS_*` keyed entries and pass the repository validator.

## RimWorld translation-report warning

RimWorld's warning:

```text
Translation data for language French has 36 errors. Generate translation report for more info.
```

was investigated with a native `TranslationReport.txt` generated in a no-DLC test configuration containing only Core, Harmony and Defensive Stances.

The report contains no `DS_*` key, no `DefensiveStances` entry and no mod-owned XML filename. The 36 def-injection errors and the four missing injected translations belong to Core French language data. They include targets such as `PsychicAmplifier`, `PsychicEntropyMax`, `Apparel_Cape`, `Humanlike.root.children...` and `ThreatReward_Raid_Joiner...`.

Defensive Stances does not patch Core translations to silence those warnings. The repository validator remains responsible only for files owned by this mod.

To locate a newly generated report during future compatibility passes, run:

```powershell
./tools/find-translation-report.ps1
```
