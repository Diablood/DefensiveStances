param(
    [string]$RimWorldManagedDir = "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed",
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",
    [switch]$SkipBuild
)

$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
$aboutPath = Join-Path $root "About\About.xml"
$assemblyPath = Join-Path $root "1.6\Assemblies\DefensiveStances.dll"
$distPath = Join-Path $root "dist"
$stagePath = Join-Path $distPath "stage"
$packageRoot = Join-Path $stagePath "DefensiveStances"
$translationValidator = Join-Path $root "tools\validate-translations.ps1"

if ($SkipBuild) {
    & $translationValidator
}
else {
    & (Join-Path $root "build.ps1") `
        -RimWorldManagedDir $RimWorldManagedDir `
        -Configuration $Configuration
}

if (-not (Test-Path $assemblyPath)) {
    throw "Compiled assembly not found: $assemblyPath. Run build.ps1 first or omit -SkipBuild."
}

[xml]$about = Get-Content $aboutPath
$version = [string]$about.ModMetaData.modVersion.InnerText

if ([string]::IsNullOrWhiteSpace($version)) {
    throw "Unable to read modVersion from $aboutPath."
}

$assemblyVersion = (Get-Item $assemblyPath).VersionInfo.FileVersion
$expectedAssemblyVersion = "$version.0"
if ($assemblyVersion -ne $expectedAssemblyVersion) {
    throw "Assembly version mismatch. About.xml contains $version, but DefensiveStances.dll reports $assemblyVersion. Rebuild before packaging."
}

if (Test-Path $stagePath) {
    Remove-Item $stagePath -Recurse -Force
}

New-Item $packageRoot -ItemType Directory -Force | Out-Null
New-Item (Join-Path $packageRoot "1.6\Assemblies") -ItemType Directory -Force | Out-Null

Copy-Item (Join-Path $root "About") $packageRoot -Recurse
Copy-Item (Join-Path $root "1.6\Languages") (Join-Path $packageRoot "1.6") -Recurse
Copy-Item (Join-Path $root "1.6\Patches") (Join-Path $packageRoot "1.6") -Recurse
Copy-Item (Join-Path $root "1.6\Textures") (Join-Path $packageRoot "1.6") -Recurse
Copy-Item $assemblyPath (Join-Path $packageRoot "1.6\Assemblies\DefensiveStances.dll")
Copy-Item (Join-Path $root "LoadFolders.xml") $packageRoot
Copy-Item (Join-Path $root "LICENSE") $packageRoot
Copy-Item (Join-Path $root "README.md") $packageRoot

$archivePath = Join-Path $distPath "DefensiveStances-$version.zip"
if (Test-Path $archivePath) {
    Remove-Item $archivePath -Force
}

Compress-Archive -Path $packageRoot -DestinationPath $archivePath -CompressionLevel Optimal
Remove-Item $stagePath -Recurse -Force

Write-Host "Release package created: $archivePath"
