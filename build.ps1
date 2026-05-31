param(
    [string]$RimWorldManagedDir = "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed",
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$project = Join-Path $PSScriptRoot "Source\DefensiveStances\DefensiveStances.csproj"

dotnet build $project `
    --configuration $Configuration `
    -p:RimWorldManagedDir="$RimWorldManagedDir"
