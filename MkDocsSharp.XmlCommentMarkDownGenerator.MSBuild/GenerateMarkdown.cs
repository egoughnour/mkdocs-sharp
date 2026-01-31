using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using MSBuildTask = Microsoft.Build.Utilities.Task;

namespace MkDocsSharp.XmlCommentMarkDownGenerator.MSBuild
{
    /// <summary>
    /// MSBuild logger adapter that bridges to TaskLoggingHelper
    /// </summary>
    internal class MSBuildLoggerAdapter : IMarkdownGeneratorLogger
    {
        private readonly TaskLoggingHelper _log;

        public MSBuildLoggerAdapter(TaskLoggingHelper log)
        {
            _log = log;
        }

        public void LogError(string message) => _log.LogError(message);
        public void LogWarning(Exception exception) => _log.LogWarningFromException(exception);
        public void LogError(Exception exception) => _log.LogErrorFromException(exception);
    }

    /// <summary>
    /// A task that generates and optionally merges markdown
    /// </summary>
    public class GenerateMarkdown : MSBuildTask
    {
        /// <summary>
        /// The file(s) from which to generate markdown.  This should be in XmlDocumentation format.
        /// </summary>
        [Required]
        public ITaskItem[] InputXml { get; set; } = Array.Empty<ITaskItem>();

        /// <summary>
        /// DocumentationPath is the top level directory in which to search for files.
        /// It is also the path where generated markdown files are created.
        /// </summary>
        [Required]
        public ITaskItem? DocumentationPath { get; set; }

        /// <summary>
        /// Whether the generated markdown files should merge.  Only valid if multiple markdown files exist.
        /// DocumentationPath is the top level directory in which to search for files.
        /// Both existing markdown files and the generated files are merged.
        /// </summary>
        public bool MergeFiles { get; set; }

        /// <summary>
        /// The file to be created by the merge.  Unused if MergeFiles evaluates to false.
        /// </summary>
        public ITaskItem? OutputFile { get; set; }

        /// <summary>
        /// Defaults to false. When true unexpected tags in the documentation
        /// will generate warnings rather than errors. 
        /// </summary>
        public bool WarnOnUnexpectedTag { get; set; } = false;

        /// <summary>
        /// for testing.  
        /// sets the exception for throw outside the catch
        /// </summary>
        public Exception? LoggedException => _generator?.LoggedException;

        /// <summary>
        /// The files generated during execution of the task
        /// </summary>
        public List<string> GeneratedMDFiles => _generator?.GeneratedMDFiles ?? new List<string>();

        private MarkdownGenerator? _generator;

        /// <summary>
        /// Runs the task as configured
        /// </summary>
        /// <returns>true if task has succeeded</returns>
        public override bool Execute()
        {
            _generator = new MarkdownGenerator(new MSBuildLoggerAdapter(Log))
            {
                InputXmlFiles = InputXml.Select(x => x.ItemSpec).ToArray(),
                DocumentationPath = DocumentationPath?.ItemSpec,
                MergeFiles = MergeFiles,
                OutputFile = OutputFile?.ItemSpec,
                WarnOnUnexpectedTag = WarnOnUnexpectedTag
            };

            return _generator.Execute();
        }

        /// <summary>
        /// Use this to handle front matter in markdown files (kept for backward compatibility)
        /// </summary>
        public static bool TryGetFrontMatter(string filePath, out string frontMatter, out bool isEmpty)
            => MarkdownGenerator.TryGetFrontMatter(filePath, out frontMatter, out isEmpty);
    }
}
