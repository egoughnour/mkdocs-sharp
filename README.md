# MkDocsSharp.MDGen

[![NuGet](https://img.shields.io/nuget/v/MkDocsSharp.MDGen.svg)](https://www.nuget.org/packages/MkDocsSharp.MDGen)
[![CI](https://github.com/egoughnour/mkdocs-sharp/actions/workflows/ci.yml/badge.svg)](https://github.com/egoughnour/mkdocs-sharp/actions/workflows/ci.yml)

Automatically generate MkDocs-friendly Markdown from your C# XML documentation comments. Runs as an MSBuild task during your build process.

## Installation

```bash
dotnet add package MkDocsSharp.MDGen
```

## Quick Start

1. Enable XML documentation in your project:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

2. Build your project. The package automatically generates `docs/index.md` from your XML docs.

That's it! Your XML documentation comments are now converted to Markdown on every build, ready for MkDocs.

## Features

- Converts all standard XML documentation tags (`<summary>`, `<param>`, `<returns>`, `<example>`, etc.)
- Outputs directly to `docs/` for seamless MkDocs integration
- Merges generated docs with existing Markdown files in `docs/`
- Supports cross-references with `<see cref="..."/>` converted to Markdown links
- Handles generic types, interfaces, enums, records, and structs
- Runs automatically as part of your build process

## Configuration

The default behavior works for most projects. The generated output goes to `docs/index.md`, and any existing `.md` files in `docs/` are merged into the output.

To customize, you can override the MSBuild target in your `.csproj`:

```xml
<Target Name="AfterBuild" DependsOnTargets="GenerateMarkdownDocs">
  <GenerateMarkdown
    InputXml="$(OutputPath)$(AssemblyName).xml"
    DocumentationPath="$(MSBuildProjectDirectory)\Docs"
    MergeFiles="true"
    OutputFile="$(MSBuildProjectDirectory)\API.md"
    WarnOnUnexpectedTag="true"
  />
</Target>
```

| Property | Default | Description |
|----------|---------|-------------|
| `InputXml` | `$(OutputPath)$(AssemblyName).xml` | XML documentation file(s) to process |
| `DocumentationPath` | `docs` | Folder containing existing Markdown to merge |
| `MergeFiles` | `true` | Whether to merge with existing docs |
| `OutputFile` | `docs/index.md` | Output Markdown file path |
| `WarnOnUnexpectedTag` | `false` | Warn on unrecognized XML tags |

## CLI Usage

The package also includes a standalone command-line tool:

```bash
MkDocsSharp.MDGen -i MyAssembly.xml -o API.md
```

| Option | Description |
|--------|-------------|
| `-i`, `--inputfile` | Input XML file |
| `-o`, `--outputfile` | Output Markdown file |
| `--cin` | Read from stdin |
| `--cout` | Write to stdout |

## Requirements

- .NET 8.0 or later
- XML documentation enabled in your project

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for development setup and release process.

## License

MIT - see [LICENSE](LICENSE) for details.

---

Originally forked from [lontivero's gist](https://gist.github.com/lontivero/593fc51f1208555112e0).
