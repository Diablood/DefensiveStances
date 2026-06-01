# Translation audit for 0.4.1-dev

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

The 0.4.1-dev translation files contain matching English and French `DS_*` keyed entries and pass the repository validator.

## RimWorld translation-report warning

RimWorld's warning:

```text
Translation data for language French has 36 errors. Generate translation report for more info.
```

cannot be attributed from `Player.log` alone. RimWorld's own report covers all loaded language data, including Core. The repository validator intentionally does not claim that every warning comes from Defensive Stances.

After generating `TranslationReport.txt` from RimWorld's developer tools, locate it with:

```powershell
./tools/find-translation-report.ps1
```

Review the generated report before the first stable release and classify each remaining error as Core-owned or Defensive-Stances-owned.
