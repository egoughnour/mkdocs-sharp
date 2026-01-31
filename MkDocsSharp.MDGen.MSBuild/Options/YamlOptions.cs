using YamlDotNet.Serialization;

namespace MkDocsSharp.MDGen.MSBuild.Options
{
    /// <summary>
    /// Specifies the manner in which custom tags will be handled
    /// </summary>
    public enum AllowedTagOptions
    { 
        /// <summary>
        /// All custom tags are allowed
        /// </summary>
        All,
        /// <summary>
        /// No custom tags are allowed
        /// </summary>
        None
    }

    /// <summary>
    /// The options to be deserialized from the front matter found.
    /// </summary>
    public class YamlOptions
    {
        /// <summary>
        /// Whether to merge XML comments into a single file.
        /// </summary>
        [YamlMember(Alias = "MergeXmlComments", ApplyNamingConventions = false)]
        public bool MergeXmlComments { get; set; }

        /// <summary>
        /// The allowed custom tags setting (All or None).
        /// </summary>
        [YamlMember(Alias = "AllowedCustomTags", ApplyNamingConventions = false)]
        public string? AllowedCustomTags { get; set; }
    }
}
