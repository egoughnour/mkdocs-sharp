using System.Xml.Linq;
using System.Xml;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using PxtlCa.XmlCommentMarkDownGenerator.MSBuild.Options;

namespace PxtlCa.XmlCommentMarkDownGenerator.MSBuild
{
    /// <summary>
    /// Logging interface to abstract MSBuild logging
    /// </summary>
    public interface IMarkdownGeneratorLogger
    {
        /// <summary>
        /// Log an error message
        /// </summary>
        void LogError(string message);

        /// <summary>
        /// Log a warning from an exception
        /// </summary>
        void LogWarning(Exception exception);

        /// <summary>
        /// Log an error from an exception
        /// </summary>
        void LogError(Exception exception);
    }

    /// <summary>
    /// Core markdown generation logic, decoupled from MSBuild
    /// </summary>
    public class MarkdownGenerator
    {
        private readonly IMarkdownGeneratorLogger _logger;

        /// <summary>
        /// Creates a new markdown generator
        /// </summary>
        /// <param name="logger">The logger to use for errors and warnings</param>
        public MarkdownGenerator(IMarkdownGeneratorLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// The input XML files to process
        /// </summary>
        public string[] InputXmlFiles { get; set; } = Array.Empty<string>();

        /// <summary>
        /// The documentation path (either a file or directory)
        /// </summary>
        public string? DocumentationPath { get; set; }

        /// <summary>
        /// Whether to merge output files
        /// </summary>
        public bool MergeFiles { get; set; }

        /// <summary>
        /// The output file path when merging
        /// </summary>
        public string? OutputFile { get; set; }

        /// <summary>
        /// Whether to warn instead of error on unexpected tags
        /// </summary>
        public bool WarnOnUnexpectedTag { get; set; }

        /// <summary>
        /// Exception logged during execution (for testing)
        /// </summary>
        public Exception? LoggedException { get; set; }

        /// <summary>
        /// The files generated during execution
        /// </summary>
        public List<string> GeneratedMDFiles { get; } = new List<string>();

        /// <summary>
        /// Whether the DocumentationPath is a file (vs directory)
        /// </summary>
        public bool DocumentationPathIsFile => DocumentationPath != null && File.Exists(DocumentationPath);

        /// <summary>
        /// Executes the markdown generation
        /// </summary>
        /// <returns>true if successful</returns>
        public bool Execute()
        {
            if (DocumentationPath == null)
            {
                _logger.LogError("DocumentationPath must be specified");
                return false;
            }

            if (InputXmlFiles.Length == 0)
            {
                _logger.LogError("InputXml cannot be empty");
            }
            else
            {
                UpdateParametersFromInput();
                if (MergeFiles && OutputFile == null)
                {
                    _logger.LogError("OutputFile must be specified if input files are merged");
                }
                else if (DocumentationPathIsFile && InputXmlFiles.Length != 1)
                {
                    _logger.LogError("DocumentationPath must specify a directory if more than one input XML value is supplied");
                }
                else
                {
                    try
                    {
                        CreateDirectoryIfNeeded();
                        GenerateFiles();
                        if (MergeFiles)
                        {
                            Merge();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        LoggedException = ex;
                        _logger.LogError(ex);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Updates parameters from YAML front matter in input files
        /// </summary>
        public void UpdateParametersFromInput()
        {
            if (DocumentationPath == null) return;

            if (DocumentationPathIsFile)
            {
                if (TryGetFrontMatter(DocumentationPath, out string frontMatter, out bool isEmpty) &&
                    (!isEmpty))
                {
                    ReadOptionsFromString(frontMatter);
                    return;
                }
            }
            else
            {
                var mdFiles = Directory.EnumerateFiles(DocumentationPath, "*.md", SearchOption.AllDirectories).ToList();
                foreach (var mdFile in mdFiles)
                {
                    if (TryGetFrontMatter(mdFile, out string frontMatter, out bool isEmpty) &&
                    (!isEmpty))
                    {
                        ReadOptionsFromString(frontMatter);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Use this to handle front matter in markdown files
        /// </summary>
        /// <param name="filePath">the path to the file</param>
        /// <param name="frontMatter">the front matter found</param>
        /// <param name="isEmpty">whether the front matter found is trivial</param>
        /// <returns>true if front matter indicator(s) are found</returns>
        public static bool TryGetFrontMatter(string filePath, out string frontMatter, out bool isEmpty)
        {
            var lines = File.ReadLines(filePath);
            var firstDashedLine = lines.FirstOrDefault() ?? string.Empty;
            if (firstDashedLine.StartsWith("---"))
            {
                var followingLines = lines.Skip(1).TakeWhile(line => !line.StartsWith("---")).ToList();
                if (followingLines.Count == 0)
                {
                    frontMatter = firstDashedLine;
                    isEmpty = true;
                    return true;
                }
                else
                {
                    followingLines.Insert(0, firstDashedLine);
                    frontMatter = String.Join(Environment.NewLine, followingLines);
                    isEmpty = false;
                    return true;
                }
            }
            frontMatter = string.Empty;
            isEmpty = true;
            return false;
        }

        private void ReadOptionsFromString(string frontMatter)
        {
            var input = new StringReader(frontMatter);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var options = deserializer.Deserialize<YamlOptions>(input);
            if (options != null)
            {
                MergeFiles = options.MergeXmlComments;
                if (!string.IsNullOrEmpty(options.AllowedCustomTags) &&
                    Enum.TryParse<AllowedTagOptions>(options.AllowedCustomTags, true, out AllowedTagOptions result))
                {
                    WarnOnUnexpectedTag = result == AllowedTagOptions.All;
                }
            }
        }

        private void Merge()
        {
            if (DocumentationPath == null || OutputFile == null) return;

            var otherMDFiles = Directory.EnumerateFiles(DocumentationPath, "*.md", SearchOption.AllDirectories).ToList();
            otherMDFiles = otherMDFiles.Except(GeneratedMDFiles).ToList();
            var mergeInto = otherMDFiles.FirstOrDefault();
            if (mergeInto == null)
            {
                mergeInto = GeneratedMDFiles.First();
                File.Copy(mergeInto, OutputFile, true);
                foreach (var mdFile in GeneratedMDFiles.Skip(1))
                {
                    File.AppendAllText(OutputFile, Environment.NewLine);
                    File.AppendAllText(OutputFile, File.ReadAllText(mdFile));
                }
            }
            else
            {
                File.Copy(mergeInto, OutputFile, true);
                foreach (var mdFile in otherMDFiles.Skip(1))
                {
                    File.AppendAllText(OutputFile, Environment.NewLine);
                    File.AppendAllText(OutputFile, File.ReadAllText(mdFile));
                }
                foreach (var mdFile in GeneratedMDFiles)
                {
                    File.AppendAllText(OutputFile, Environment.NewLine);
                    File.AppendAllText(OutputFile, File.ReadAllText(mdFile));
                }
            }
        }

        private void GenerateFiles()
        {
            if (DocumentationPath == null) return;

            foreach (var inputFile in InputXmlFiles)
            {
                try
                {
                    var mdOutput = GetOutputPath(inputFile);
                    GeneratedMDFiles.Add(mdOutput);
                    var sr = new StreamReader(inputFile);
                    using (var sw = new StreamWriter(mdOutput))
                    {
                        var xml = sr.ReadToEnd();
                        var doc = XDocument.Parse(xml);
                        var md = doc.Root?.ToMarkDown() ?? string.Empty;
                        sw.Write(md);
                        sw.Close();
                    }
                }
                catch (XmlException xmlException)
                {
                    if (WarnOnUnexpectedTag && xmlException.InnerException != null &&
                        xmlException.InnerException.GetType() == typeof(KeyNotFoundException))
                    {
                        _logger.LogWarning(xmlException);
                        continue;
                    }
                    throw;
                }
            }
        }

        private string GetOutputPath(string inputXml)
        {
            if (DocumentationPath == null) return inputXml;

            if (DocumentationPathIsFile)
            {
                return DocumentationPath;
            }
            return Path.Combine(DocumentationPath, $"{Path.GetFileNameWithoutExtension(inputXml)}.md");
        }

        private void CreateDirectoryIfNeeded()
        {
            if (DocumentationPath != null && !DocumentationPathIsFile && !Directory.Exists(DocumentationPath))
            {
                Directory.CreateDirectory(DocumentationPath);
            }
        }
    }
}
