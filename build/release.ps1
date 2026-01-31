param(
    [Parameter(Mandatory = $true)]
    [string]$Version,
    [string]$NuGetApiKey,
    [switch]$SkipGit,
    [switch]$SkipRelease,
    [switch]$SkipPush
)

$root = Split-Path -Parent $PSScriptRoot
$nuspecPath = Join-Path $root "MkDocsSharp.MDGen.nuspec"
$packProj = Join-Path $PSScriptRoot "pack.proj"
$artifactsDir = Join-Path $root "artifacts"

if (!(Test-Path $nuspecPath)) {
    throw "Nuspec not found at $nuspecPath"
}

[xml]$nuspec = Get-Content $nuspecPath
$nuspec.package.metadata.version = $Version
$nuspec.Save($nuspecPath)

dotnet msbuild $packProj /t:Pack /p:Configuration=Release

$nupkg = Get-ChildItem $artifactsDir -Filter "MkDocsSharp.MDGen.$Version.nupkg" | Select-Object -First 1
if (-not $nupkg) {
    throw "Package not found in $artifactsDir for version $Version"
}

if (-not $SkipGit) {
    git add $nuspecPath
    git commit -m "Release v$Version"
    git tag "v$Version"
    git push
    git push --tags
}

if (-not $SkipRelease) {
    if (Get-Command gh -ErrorAction SilentlyContinue) {
        gh release create "v$Version" $nupkg.FullName --title "v$Version" --notes "Release v$Version"
    }
    else {
        Write-Warning "gh CLI not found; skipping GitHub release creation."
    }
}

if (-not $SkipPush) {
    $apiKey = if ($NuGetApiKey) { $NuGetApiKey } else { $env:NUGET_API_KEY }
    if (-not $apiKey) {
        throw "NuGet API key not provided. Use -NuGetApiKey or set NUGET_API_KEY."
    }
    dotnet nuget push $nupkg.FullName -s https://api.nuget.org/v3/index.json -k $apiKey
}
