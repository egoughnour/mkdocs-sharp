# Contributing to MkDocsSharp.MDGen

## Development Setup

Clone the repository and restore dependencies:

```bash
git clone https://github.com/egoughnour/mkdocs-sharp.git
cd mkdocs-sharp
dotnet restore MkDocsSharp.MDGen.slnx
```

## Building

```bash
dotnet build MkDocsSharp.MDGen.slnx -c Release
```

## Testing

```bash
dotnet test MkDocsSharp.MDGen.slnx
```

## Project Structure

- `MkDocsSharp.MDGen.slnx` - Main solution with shipping projects
- `UserCode.sln` - Example/sample projects (not included in package)
- `MkDocsSharp.MDGen/` - CLI tool
- `MkDocsSharp.MDGen.MSBuild/` - MSBuild task implementation

## Packaging

Build and pack locally:

```bash
dotnet build MkDocsSharp.MDGen.slnx -c Release
dotnet pack MkDocsSharp.MDGen.MSBuild -c Release -o artifacts --no-build
```

## Releasing

Releases are automated via GitHub Actions. To create a release:

1. Push a tag: `git tag v1.2.3 && git push origin v1.2.3`
2. Or manually trigger the release workflow with a version input

The workflow builds, packs, creates a GitHub release, and publishes to NuGet.org. Requires a `NUGET_API_KEY` secret in the repository.

## Local Release Scripts

For local releases (requires `gh` CLI and appropriate permissions):

```bash
./build/release.sh 1.2.3
```

Or PowerShell:

```powershell
./build/release.ps1 -Version 1.2.3
```
