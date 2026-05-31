#!/usr/bin/env bash
set -euo pipefail

RIMWORLD_MANAGED_DIR="${1:-$HOME/.steam/steam/steamapps/common/RimWorld/RimWorldLinux_Data/Managed}"
CONFIGURATION="${2:-Release}"
SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"

dotnet build "$SCRIPT_DIR/Source/DefensiveStances/DefensiveStances.csproj" \
  --configuration "$CONFIGURATION" \
  -p:RimWorldManagedDir="$RIMWORLD_MANAGED_DIR"
