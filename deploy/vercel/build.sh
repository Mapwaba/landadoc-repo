#!/usr/bin/env bash
# Vercel build step shared by the Patient, Doctor and Admin frontends.
# Run from the frontend's folder (its Vercel "Root Directory"):
#   bash ../../../deploy/vercel/build.sh LandaDoc.Patient.csproj
#
# Vercel's build image has no .NET SDK, so this installs one, publishes the
# standalone Blazor WASM app to ./publish, and swaps in the Render API URLs
# (the committed wwwroot/appsettings.Production.json targets the VPS setup).
set -euo pipefail

PROJECT="$1"
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
# The build image may lack libicu; the SDK itself doesn't need culture data.
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

if command -v dotnet >/dev/null 2>&1 && dotnet --list-sdks | grep -q '^9\.'; then
  DOTNET=dotnet
else
  DOTNET_DIR="$REPO_ROOT/.dotnet"
  if [ ! -x "$DOTNET_DIR/dotnet" ]; then
    curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
    bash /tmp/dotnet-install.sh --channel 9.0 --install-dir "$DOTNET_DIR"
  fi
  DOTNET="$DOTNET_DIR/dotnet"
fi

rm -rf publish
"$DOTNET" publish "$PROJECT" -c Release -o publish

WWWROOT=publish/wwwroot
cp "$REPO_ROOT/deploy/vercel/appsettings.Production.json" "$WWWROOT/appsettings.Production.json"
# Drop the precompressed copies of the old file so nothing can serve them.
rm -f "$WWWROOT/appsettings.Production.json.br" "$WWWROOT/appsettings.Production.json.gz"
