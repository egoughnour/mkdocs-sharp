using Microsoft.VisualStudio.TestTools.UnitTesting;
using MkDocsSharp.XmlCommentMarkDownGenerator.MSBuild;
using System.Xml;

namespace MkDocsSharp.XmlCommentMarkDownGenerator.MSBuild.Test
{
    /// <summary>
    /// Simple test logger that collects errors and warnings
    /// </summary>
    public class TestLogger : IMarkdownGeneratorLogger
    {
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();

        public void LogError(string message) => Errors.Add(message);
        public void LogWarning(Exception exception) => Warnings.Add(exception.Message);
        public void LogError(Exception exception) => Errors.Add(exception.Message);
    }

    [TestClass]
    public class MarkdownGeneratorTests
    {
        private string _testDir = null!;

        [TestInitialize]
        public void Setup()
        {
            _testDir = Path.Combine(Path.GetTempPath(), $"MarkdownGeneratorTests_{Guid.NewGuid():N}");
            Directory.CreateDirectory(_testDir);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_testDir))
            {
                Directory.Delete(_testDir, recursive: true);
            }
        }

        private string CreateTestXml(string content)
        {
            var path = Path.Combine(_testDir, $"test_{Guid.NewGuid():N}.xml");
            File.WriteAllText(path, content);
            return path;
        }

        private const string SimpleXmlDoc = @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>TestAssembly</name>
    </assembly>
    <members>
        <member name=""T:TestNamespace.TestClass"">
            <summary>A test class for unit testing.</summary>
        </member>
        <member name=""M:TestNamespace.TestClass.TestMethod(System.String)"">
            <summary>A test method.</summary>
            <param name=""input"">The input string.</param>
            <returns>The processed result.</returns>
        </member>
    </members>
</doc>";

        private const string XmlDocWithUnknownTag = @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>TestAssembly</name>
    </assembly>
    <members>
        <member name=""T:TestNamespace.TestClass"">
            <summary>A test class.</summary>
            <completelyunknowntag>Unknown content</completelyunknowntag>
        </member>
    </members>
</doc>";

        [TestMethod]
        public void Execute_WithValidXml_GeneratesMarkdown()
        {
            // Arrange
            var logger = new TestLogger();
            var inputPath = CreateTestXml(SimpleXmlDoc);
            var outputDir = Path.Combine(_testDir, "output");
            Directory.CreateDirectory(outputDir);

            var generator = new MarkdownGenerator(logger)
            {
                InputXmlFiles = new[] { inputPath },
                DocumentationPath = outputDir,
                MergeFiles = false
            };

            // Act
            var result = generator.Execute();

            // Assert
            Assert.IsTrue(result, "Execute should return true");
            Assert.AreEqual(0, logger.Errors.Count, "Should have no errors");
            Assert.AreEqual(1, generator.GeneratedMDFiles.Count, "Should generate one file");
            Assert.IsTrue(File.Exists(generator.GeneratedMDFiles[0]), "Generated file should exist");

            var content = File.ReadAllText(generator.GeneratedMDFiles[0]);
            Assert.IsTrue(content.Contains("TestClass"), "Output should contain class name");
            Assert.IsTrue(content.Contains("TestMethod"), "Output should contain method name");
        }

        [TestMethod]
        public void Execute_WithMerge_CombinesFiles()
        {
            // Arrange
            var logger = new TestLogger();
            var inputPath = CreateTestXml(SimpleXmlDoc);
            var outputDir = Path.Combine(_testDir, "output");
            var outputFile = Path.Combine(_testDir, "merged.md");
            Directory.CreateDirectory(outputDir);

            // Create an existing markdown file to merge with
            File.WriteAllText(Path.Combine(outputDir, "existing.md"), "# Existing Content\n\nSome existing documentation.");

            var generator = new MarkdownGenerator(logger)
            {
                InputXmlFiles = new[] { inputPath },
                DocumentationPath = outputDir,
                MergeFiles = true,
                OutputFile = outputFile
            };

            // Act
            var result = generator.Execute();

            // Assert
            Assert.IsTrue(result, "Execute should return true");
            Assert.IsTrue(File.Exists(outputFile), "Merged file should exist");

            var content = File.ReadAllText(outputFile);
            Assert.IsTrue(content.Contains("Existing Content"), "Should contain existing content");
        }

        [TestMethod]
        public void Execute_WithUnknownTag_AndWarnOnUnexpectedTag_Warns()
        {
            // Arrange
            var logger = new TestLogger();
            var inputPath = CreateTestXml(XmlDocWithUnknownTag);
            var outputDir = Path.Combine(_testDir, "output");
            Directory.CreateDirectory(outputDir);

            var generator = new MarkdownGenerator(logger)
            {
                InputXmlFiles = new[] { inputPath },
                DocumentationPath = outputDir,
                MergeFiles = false,
                WarnOnUnexpectedTag = true
            };

            // Act
            var result = generator.Execute();

            // Assert
            Assert.IsTrue(result, "Execute should return true when warning on unexpected tags");
            Assert.IsTrue(logger.Warnings.Count > 0, "Should have logged a warning");
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void Execute_WithUnknownTag_AndNoWarn_Throws()
        {
            // Arrange
            var logger = new TestLogger();
            var inputPath = CreateTestXml(XmlDocWithUnknownTag);
            var outputDir = Path.Combine(_testDir, "output");
            Directory.CreateDirectory(outputDir);

            var generator = new MarkdownGenerator(logger)
            {
                InputXmlFiles = new[] { inputPath },
                DocumentationPath = outputDir,
                MergeFiles = false,
                WarnOnUnexpectedTag = false
            };

            // Act
            generator.Execute();

            // Throw the logged exception to match expected behavior
            if (generator.LoggedException != null)
            {
                throw generator.LoggedException;
            }
        }

        [TestMethod]
        public void Execute_WithNoDocumentationPath_LogsError()
        {
            // Arrange
            var logger = new TestLogger();

            var generator = new MarkdownGenerator(logger)
            {
                InputXmlFiles = new[] { "test.xml" },
                DocumentationPath = null
            };

            // Act
            var result = generator.Execute();

            // Assert
            Assert.IsFalse(result, "Execute should return false");
            Assert.IsTrue(logger.Errors.Any(e => e.Contains("DocumentationPath")), "Should log error about DocumentationPath");
        }

        [TestMethod]
        public void Execute_WithNoInputXml_LogsError()
        {
            // Arrange
            var logger = new TestLogger();
            var outputDir = Path.Combine(_testDir, "output");
            Directory.CreateDirectory(outputDir);

            var generator = new MarkdownGenerator(logger)
            {
                InputXmlFiles = Array.Empty<string>(),
                DocumentationPath = outputDir
            };

            // Act
            var result = generator.Execute();

            // Assert
            Assert.IsFalse(result, "Execute should return false");
            Assert.IsTrue(logger.Errors.Any(e => e.Contains("InputXml")), "Should log error about InputXml");
        }

        [TestMethod]
        public void Execute_WithMergeButNoOutputFile_LogsError()
        {
            // Arrange
            var logger = new TestLogger();
            var inputPath = CreateTestXml(SimpleXmlDoc);
            var outputDir = Path.Combine(_testDir, "output");
            Directory.CreateDirectory(outputDir);

            var generator = new MarkdownGenerator(logger)
            {
                InputXmlFiles = new[] { inputPath },
                DocumentationPath = outputDir,
                MergeFiles = true,
                OutputFile = null
            };

            // Act
            var result = generator.Execute();

            // Assert
            Assert.IsFalse(result, "Execute should return false");
            Assert.IsTrue(logger.Errors.Any(e => e.Contains("OutputFile")), "Should log error about OutputFile");
        }

        [TestMethod]
        public void TryGetFrontMatter_WithValidFrontMatter_ReturnsTrueAndContent()
        {
            // Arrange
            var mdPath = Path.Combine(_testDir, "test.md");
            File.WriteAllText(mdPath, @"---
MergeXmlComments: true
AllowedCustomTags: all
---
# Content
Some markdown content.");

            // Act
            var result = MarkdownGenerator.TryGetFrontMatter(mdPath, out string frontMatter, out bool isEmpty);

            // Assert
            Assert.IsTrue(result, "Should return true for file with front matter");
            Assert.IsFalse(isEmpty, "Should not be empty");
            Assert.IsTrue(frontMatter.Contains("MergeXmlComments"), "Should contain front matter content");
        }

        [TestMethod]
        public void TryGetFrontMatter_WithEmptyFrontMatter_ReturnsIsEmpty()
        {
            // Arrange
            var mdPath = Path.Combine(_testDir, "test.md");
            File.WriteAllText(mdPath, @"---
---
# Content");

            // Act
            var result = MarkdownGenerator.TryGetFrontMatter(mdPath, out string frontMatter, out bool isEmpty);

            // Assert
            Assert.IsTrue(result, "Should return true");
            Assert.IsTrue(isEmpty, "Should be empty");
        }

        [TestMethod]
        public void TryGetFrontMatter_WithNoFrontMatter_ReturnsFalse()
        {
            // Arrange
            var mdPath = Path.Combine(_testDir, "test.md");
            File.WriteAllText(mdPath, @"# Content
Some markdown without front matter.");

            // Act
            var result = MarkdownGenerator.TryGetFrontMatter(mdPath, out string frontMatter, out bool isEmpty);

            // Assert
            Assert.IsFalse(result, "Should return false for file without front matter");
        }
    }
}
