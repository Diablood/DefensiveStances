param(
    [string]$LanguagesDir = (Join-Path $PSScriptRoot "..\1.6\Languages")
)

$ErrorActionPreference = "Stop"
$requiredLanguages = @("English", "French")
$errors = New-Object System.Collections.Generic.List[string]
$languageKeys = @{}

function Add-ValidationError {
    param([string]$Message)

    [void]$errors.Add($Message)
}

function Read-KeyedTranslations {
    param(
        [string]$Language,
        [string]$KeyedDir
    )

    $translations = @{}
    $files = @(Get-ChildItem -Path $KeyedDir -Filter "*.xml" -File -Recurse | Sort-Object FullName)

    if ($files.Count -eq 0) {
        Add-ValidationError "${Language}: no keyed translation XML file found in '$KeyedDir'."
        return $translations
    }

    foreach ($file in $files) {
        try {
            [xml]$xml = Get-Content -Path $file.FullName -Raw -Encoding UTF8
        }
        catch {
            Add-ValidationError "${Language}: invalid XML in '$($file.FullName)': $($_.Exception.Message)"
            continue
        }

        if ($null -eq $xml.LanguageData) {
            Add-ValidationError "${Language}: '$($file.FullName)' must use <LanguageData> as its root element."
            continue
        }

        foreach ($node in @($xml.LanguageData.ChildNodes)) {
            if ($node.NodeType -ne [System.Xml.XmlNodeType]::Element) {
                continue
            }

            $key = $node.Name
            $value = $node.InnerText

            if ($translations.ContainsKey($key)) {
                Add-ValidationError "${Language}: duplicate key '$key' in '$($file.FullName)'."
                continue
            }

            if ([string]::IsNullOrWhiteSpace($value)) {
                Add-ValidationError "${Language}: key '$key' has an empty translation in '$($file.FullName)'."
            }
            elseif ($value -eq "TODO") {
                Add-ValidationError "${Language}: key '$key' still contains the TODO placeholder in '$($file.FullName)'."
            }

            $translations[$key] = $value
        }
    }

    return $translations
}

foreach ($language in $requiredLanguages) {
    $keyedDir = Join-Path $LanguagesDir "$language\Keyed"

    if (-not (Test-Path -Path $keyedDir -PathType Container)) {
        Add-ValidationError "${language}: missing keyed translation directory '$keyedDir'."
        $languageKeys[$language] = @{}
        continue
    }

    $languageKeys[$language] = Read-KeyedTranslations -Language $language -KeyedDir $keyedDir
}

$englishKeys = @($languageKeys["English"].Keys | Sort-Object)

foreach ($language in ($requiredLanguages | Where-Object { $_ -ne "English" })) {
    $currentKeys = @($languageKeys[$language].Keys | Sort-Object)
    $missingKeys = @($englishKeys | Where-Object { -not $languageKeys[$language].ContainsKey($_) })
    $extraKeys = @($currentKeys | Where-Object { -not $languageKeys["English"].ContainsKey($_) })

    foreach ($key in $missingKeys) {
        Add-ValidationError "${language}: missing key '$key' present in English."
    }

    foreach ($key in $extraKeys) {
        Add-ValidationError "${language}: extra key '$key' absent from English."
    }
}

if ($errors.Count -gt 0) {
    Write-Host "Translation validation failed:" -ForegroundColor Red

    foreach ($errorMessage in $errors) {
        Write-Host "  - $errorMessage" -ForegroundColor Red
    }

    exit 1
}

Write-Host "Translation validation passed: $($englishKeys.Count) English keyed entries and matching French entries." -ForegroundColor Green
