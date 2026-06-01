#!/usr/bin/env bash
set -euo pipefail

RIMWORLD_MANAGED_DIR="${1:-$HOME/.steam/steam/steamapps/common/RimWorld/RimWorldLinux_Data/Managed}"
CONFIGURATION="${2:-Release}"
SKIP_BUILD="${SKIP_BUILD:-0}"
SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd -- "$SCRIPT_DIR/.." && pwd)"

if [[ "$SKIP_BUILD" != "1" ]]; then
    "$ROOT_DIR/build.sh" "$RIMWORLD_MANAGED_DIR" "$CONFIGURATION"
elif command -v pwsh >/dev/null 2>&1; then
    pwsh -NoProfile -File "$ROOT_DIR/tools/validate-translations.ps1"
else
    printf 'Skipping translation validation: pwsh was not found.\n' >&2
fi

python3 - "$ROOT_DIR" <<'PY'
from __future__ import annotations

import shutil
import sys
import xml.etree.ElementTree as ET
import zipfile
from pathlib import Path

root = Path(sys.argv[1])
about_path = root / "About" / "About.xml"
assembly_path = root / "1.6" / "Assemblies" / "DefensiveStances.dll"
dist_path = root / "dist"
stage_path = dist_path / "stage"
package_root = stage_path / "DefensiveStances"

if not assembly_path.is_file():
    raise SystemExit(f"Compiled assembly not found: {assembly_path}. Run build.sh first or set SKIP_BUILD=1 only after compiling.")

version = ET.parse(about_path).getroot().findtext("modVersion")
if not version:
    raise SystemExit(f"Unable to read modVersion from {about_path}.")

if stage_path.exists():
    shutil.rmtree(stage_path)
(package_root / "1.6" / "Assemblies").mkdir(parents=True)

for source, target in [
    (root / "About", package_root / "About"),
    (root / "1.6" / "Languages", package_root / "1.6" / "Languages"),
    (root / "1.6" / "Patches", package_root / "1.6" / "Patches"),
    (root / "1.6" / "Textures", package_root / "1.6" / "Textures"),
]:
    shutil.copytree(source, target)

shutil.copy2(assembly_path, package_root / "1.6" / "Assemblies" / "DefensiveStances.dll")
for filename in ["LoadFolders.xml", "LICENSE", "README.md"]:
    shutil.copy2(root / filename, package_root / filename)

archive_path = dist_path / f"DefensiveStances-{version}.zip"
dist_path.mkdir(exist_ok=True)
if archive_path.exists():
    archive_path.unlink()

with zipfile.ZipFile(archive_path, "w", zipfile.ZIP_DEFLATED) as archive:
    for path in sorted(package_root.rglob("*")):
        if path.is_file():
            archive.write(path, path.relative_to(stage_path))

shutil.rmtree(stage_path)
print(f"Release package created: {archive_path}")
PY
