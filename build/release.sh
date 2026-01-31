#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <version>"
  exit 1
fi

VERSION="$1"
ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
NUSPEC_PATH="$ROOT_DIR/MkDocsSharp.MDGen.nuspec"
PACK_PROJ="$ROOT_DIR/build/pack.proj"
ARTIFACTS_DIR="$ROOT_DIR/artifacts"

if [[ ! -f "$NUSPEC_PATH" ]]; then
  echo "Nuspec not found at $NUSPEC_PATH"
  exit 1
fi

perl -0pi -e "s/<version>[^<]*<\\/version>/<version>${VERSION}<\\/version>/" "$NUSPEC_PATH"

dotnet msbuild "$PACK_PROJ" /t:Pack /p:Configuration=Release

NUPKG="$ARTIFACTS_DIR/MkDocsSharp.MDGen.$VERSION.nupkg"
if [[ ! -f "$NUPKG" ]]; then
  echo "Package not found at $NUPKG"
  exit 1
fi

if [[ "${SKIP_GIT:-}" != "1" ]]; then
  git add "$NUSPEC_PATH"
  git commit -m "Release v$VERSION"
  git tag "v$VERSION"
  git push
  git push --tags
fi

if [[ "${SKIP_RELEASE:-}" != "1" ]]; then
  if command -v gh >/dev/null 2>&1; then
    gh release create "v$VERSION" "$NUPKG" --title "v$VERSION" --notes "Release v$VERSION"
  else
    echo "gh CLI not found; skipping GitHub release creation."
  fi
fi

if [[ "${SKIP_PUSH:-}" != "1" ]]; then
  if [[ -z "${NUGET_API_KEY:-}" ]]; then
    echo "NUGET_API_KEY is not set."
    exit 1
  fi
  dotnet nuget push "$NUPKG" -s https://api.nuget.org/v3/index.json -k "$NUGET_API_KEY"
fi
