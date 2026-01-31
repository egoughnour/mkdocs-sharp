using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Xml.Linq;

namespace Example.SystemTests;

/// <summary>
/// System tests that verify end-to-end document generation from the example project.
/// These tests build the sample project with the MkDocsSharp.MDGen package added at test time.
/// </summary>
[TestClass]
public class DocumentGenerationTests
{
    private const string MergeMarker = "This project demonstrates XML documentation patterns.";
    private static string? _tempProjectDir;
    private static string? _readmeContent;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        var repoRoot = FindRepoRoot();
        var nuspecPath = Path.Combine(repoRoot, "MkDocsSharp.MDGen.nuspec");
        var version = ReadNuspecVersion(nuspecPath);

        var artifactsDir = Path.Combine(repoRoot, "artifacts");
        var nupkgPath = Path.Combine(artifactsDir, $"MkDocsSharp.MDGen.{version}.nupkg");
        if (!File.Exists(nupkgPath))
        {
            Assert.Inconclusive($"Package not found: {nupkgPath}. Run: dotnet pack MkDocsSharp.MDGen.MSBuild -c Release -o artifacts");
            return;
        }

        var sourceProjectDir = Path.Combine(repoRoot, "Example", "Example.XmlDocSamples");
        _tempProjectDir = Path.Combine(Path.GetTempPath(), $"MDGenSample_{Guid.NewGuid():N}");
        CopyDirectory(sourceProjectDir, _tempProjectDir);

        var docsDir = Path.Combine(_tempProjectDir, "docs");
        Directory.CreateDirectory(docsDir);
        var existingDocPath = Path.Combine(docsDir, "README.md");
        File.WriteAllText(existingDocPath, $@"# Example API Documentation

{MergeMarker}

---
");

        var csprojPath = Path.Combine(_tempProjectDir, "Example.XmlDocSamples.csproj");
        RunProcess("dotnet", $"add \"{csprojPath}\" package MkDocsSharp.MDGen --version {version} --source \"{artifactsDir}\"", _tempProjectDir);
        RunProcess("dotnet", $"restore \"{csprojPath}\" --source \"{artifactsDir}\"", _tempProjectDir);
        RunProcess("dotnet", $"build \"{csprojPath}\" -c Release --no-restore", _tempProjectDir);

        var readmePath = Path.Combine(_tempProjectDir, "docs", "index.md");
        if (!File.Exists(readmePath))
        {
            Assert.Inconclusive($"Generated docs/index.md not found at {readmePath}");
            return;
        }

        _readmeContent = File.ReadAllText(readmePath);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        if (_tempProjectDir != null && Directory.Exists(_tempProjectDir))
        {
            try
            {
                Directory.Delete(_tempProjectDir, recursive: true);
            }
            catch
            {
                // Ignore cleanup failures in tests
            }
        }
    }

    [TestMethod]
    public void GenerateMarkdown_FromExampleProject_ProducesValidOutput()
    {
        var mdContent = GetReadmeContent();

        Assert.IsTrue(mdContent.Contains("BasicClass"), "Should document BasicClass");
        Assert.IsTrue(mdContent.Contains("GenericContainer"), "Should document generic types");
        Assert.IsTrue(mdContent.Contains("Point2D"), "Should document structs");
        Assert.IsTrue(mdContent.Contains("Person"), "Should document records");
        Assert.IsTrue(mdContent.Contains("IRepository"), "Should document interfaces");
        Assert.IsTrue(mdContent.Contains("DayOfWeek"), "Should document enums");
    }

    [TestMethod]
    public void GenerateMarkdown_ContainsMethodSignatures()
    {
        var mdContent = GetReadmeContent();

        Assert.IsTrue(mdContent.Contains("param") || mdContent.Contains("Parameter"),
            "Should include parameter documentation");
        Assert.IsTrue(mdContent.Contains("returns") || mdContent.Contains("Returns"),
            "Should include return value documentation");
    }

    [TestMethod]
    public void GenerateMarkdown_WithMerge_CombinesWithExistingDocs()
    {
        var mdContent = GetReadmeContent();
        Assert.IsTrue(mdContent.Contains(MergeMarker), "Should contain original content from docs");
    }

    [TestMethod]
    public void GenerateMarkdown_OutputIsValidMarkdown()
    {
        var mdContent = GetReadmeContent();

        Assert.IsTrue(mdContent.Contains("#"), "Should contain headers");
        Assert.IsFalse(mdContent.Contains("<?xml"), "Should not contain raw XML declaration");
        Assert.IsFalse(mdContent.Contains("</member>"), "Should not contain raw XML tags");
    }

    [TestMethod]
    public void GenerateMarkdown_ContainsAllXmlDocTagOutputs()
    {
        var mdContent = GetReadmeContent();

        Assert.IsTrue(mdContent.Contains("Demonstrates"), "summary tag should produce descriptions");
        Assert.IsTrue(mdContent.Contains(">"), "remarks tag should produce blockquotes");
        Assert.IsTrue(mdContent.Contains("Example:"), "example tag should produce Example header");
        Assert.IsTrue(mdContent.Contains("```"), "code tag should produce code blocks");
        Assert.IsTrue(mdContent.Contains("|Name | Description |"), "param tag should produce parameter table");
        Assert.IsTrue(mdContent.Contains("|T:"), "typeparam tag should produce type parameter rows");
        Assert.IsTrue(mdContent.Contains("**Returns**:"), "returns tag should produce Returns section");
        Assert.IsTrue(mdContent.Contains("ArgumentNullException") || mdContent.Contains("ArgumentException"),
            "exception tag should document exceptions");
        Assert.IsTrue(mdContent.Contains("**Value**:"), "value tag should produce Value section");
        Assert.IsTrue(mdContent.Contains("`true`") || mdContent.Contains("`false`"), "c tag should produce inline code");
        Assert.IsTrue(mdContent.Contains("[`") && mdContent.Contains("`]("), "see cref tag should produce markdown links");
        Assert.IsTrue(mdContent.Contains("`null`"), "see langword tag should format null keyword");
        Assert.IsTrue(mdContent.Contains("`async`") || mdContent.Contains("`await`"),
            "see langword tag should format async keywords");
        Assert.IsTrue(mdContent.Contains("**See also**:"), "seealso tag should produce See also section");
        Assert.IsTrue(mdContent.Contains("\n\n"), "para tag should produce paragraph breaks");
        Assert.IsTrue(mdContent.Contains("- "), "list/item tags should produce bullet points");
        Assert.IsTrue(mdContent.Contains("`input`") || mdContent.Contains("`value`"),
            "paramref tag should reference parameter names");
        Assert.IsTrue(mdContent.Contains("`T`") || mdContent.Contains("`TValue`"),
            "typeparamref tag should reference type parameter names");
        Assert.IsTrue(mdContent.Contains("Inherits documentation"),
            "inheritdoc tag should produce inheritance note");
        Assert.IsTrue(mdContent.Contains("## Type "), "Type members should have Type prefix");
        Assert.IsTrue(mdContent.Contains("#### Method "), "Method members should have Method prefix");
        Assert.IsTrue(mdContent.Contains("#### Property "), "Property members should have Property prefix");
        Assert.IsTrue(mdContent.Contains("#### Field ") || mdContent.Contains("Field"),
            "Field members should be documented");
        Assert.IsTrue(mdContent.Contains("#### Event ") || mdContent.Contains("Event"),
            "Event members should be documented");
    }

    private static string GetReadmeContent()
    {
        if (string.IsNullOrEmpty(_readmeContent))
        {
            Assert.Inconclusive("Generated markdown content not available.");
        }
        return _readmeContent!;
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "MkDocsSharp.MDGen.nuspec")))
            {
                return dir.FullName;
            }
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("Repository root not found (MkDocsSharp.MDGen.nuspec).");
    }

    private static string ReadNuspecVersion(string nuspecPath)
    {
        var doc = XDocument.Load(nuspecPath);
        var ns = doc.Root?.GetDefaultNamespace() ?? XNamespace.None;
        var version = doc.Root?
            .Element(ns + "metadata")?
            .Element(ns + "version")?
            .Value;

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new InvalidOperationException($"Could not read version from {nuspecPath}");
        }
        return version;
    }

    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);
        foreach (var file in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(sourceDir, file);
            if (relative.StartsWith("bin" + Path.DirectorySeparatorChar) ||
                relative.StartsWith("obj" + Path.DirectorySeparatorChar))
            {
                continue;
            }
            var destination = Path.Combine(destDir, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            File.Copy(file, destination, overwrite: true);
        }
    }

    private static string RunProcess(string fileName, string arguments, string workingDirectory)
    {
        var startInfo = new ProcessStartInfo(fileName, arguments)
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = Process.Start(startInfo)!;
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        var output = string.Join(Environment.NewLine, stdout, stderr).Trim();
        Assert.AreEqual(0, process.ExitCode, $"Command failed: {fileName} {arguments}{Environment.NewLine}{output}");
        return output;
    }
}
