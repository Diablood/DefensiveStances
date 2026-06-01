param(
    [string[]]$SearchRoots
)

$ErrorActionPreference = "Stop"

if ($null -eq $SearchRoots -or $SearchRoots.Count -eq 0) {
    $SearchRoots = @(
        (Join-Path $env:USERPROFILE "AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios"),
        (Join-Path $env:USERPROFILE "AppData\LocalLow\Ludeon Studios\RimWorld"),
        (Get-Location).Path
    )
}

$reports = @(
    $SearchRoots |
        Where-Object { Test-Path -Path $_ -PathType Container } |
        ForEach-Object {
            Get-ChildItem -Path $_ -Filter "TranslationReport.txt" -File -Recurse -ErrorAction SilentlyContinue
        } |
        Sort-Object LastWriteTime -Descending -Unique
)

if ($reports.Count -eq 0) {
    Write-Host "No TranslationReport.txt file was found under:" -ForegroundColor Yellow

    foreach ($searchRoot in $SearchRoots) {
        Write-Host "  $searchRoot"
    }

    Write-Host "Generate a translation report from RimWorld's developer tools, then run this script again."
    Write-Host "For a custom save-data folder, pass it explicitly:"
    Write-Host "  ./tools/find-translation-report.ps1 -SearchRoots 'D:\\Path\\To\\RimWorldSaveData'"
    exit 1
}

Write-Host "Translation report files found:" -ForegroundColor Green

foreach ($report in $reports) {
    Write-Host "  $($report.FullName)"
    Write-Host "    Last modified: $($report.LastWriteTime)"
}

$latestReport = $reports[0]
Write-Host ""
Write-Host "Open the newest report with Cursor:" -ForegroundColor Cyan
Write-Host "  cursor `"$($latestReport.FullName)`""
