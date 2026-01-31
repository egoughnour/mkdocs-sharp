using System.Xml.Linq;

namespace MkDocsSharp.MDGen
{
    public class TagRenderer
    {
        public TagRenderer(string formatString, Func<XElement, string?, IEnumerable<string>> valueExtractor)
        {
            FormatString = formatString;
            ValueExtractor = valueExtractor;
        }

        public string FormatString { get; } = "";

        public Func<
            XElement, //xml Element to extract from 
            string?, //assembly name
            IEnumerable<string> //resultant list of values that will get used with formatString
        > ValueExtractor;

        public static Dictionary<string, TagRenderer> Dict { get; } = new Dictionary<string, TagRenderer>()
        {
            ["doc"] = new TagRenderer(
                "# {0} #\n\n{1}\n\n",
                (x, assemblyName) => new[]{
                        x.Element("assembly")?.Element("name")?.Value ?? "",
                        x.Element("members")?.Elements("member").ToMarkDown(x.Element("assembly")?.Element("name")?.Value) ?? ""
                }
            ),
            ["type"] = new TagRenderer(
                "## {0}\n\n{1}\n\n---\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBodyFromMember(x, assemblyName)
            ),
            ["field"] = new TagRenderer(
                "#### {0}\n\n{1}\n\n---\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBodyFromMember(x, assemblyName)
            ),
            ["property"] = new TagRenderer(
                "#### {0}\n\n{1}\n\n---\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBodyFromMember(x, assemblyName)
            ),
            ["method"] = new TagRenderer(
                "#### {0}\n\n{1}\n\n---\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBodyFromMember(x, assemblyName)
            ),
            ["event"] = new TagRenderer(
                "#### {0}\n\n{1}\n\n---\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBodyFromMember(x, assemblyName)
            ),
            ["summary"] = new TagRenderer(
                "{0}\n\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["value"] = new TagRenderer(
                "**Value**: {0}\n\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["remarks"] = new TagRenderer(
                "\n\n>{0}\n\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["example"] = new TagRenderer(
                "##### Example: {0}\n\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["para"] = new TagRenderer(
                "{0}\n\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["code"] = new TagRenderer(
                "\n\n###### {0} code\n\n```\n{1}\n```\n\n",
                (x, assemblyName) => new[] { x.Attribute("lang")?.Value ?? "", x.Value.ToCodeBlock() }
            ),
            ["seePage"] = new TagRenderer(
                "[[{1}|{0}]]",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBody("cref", x, assemblyName)
            ),
            ["seeAnchor"] = new TagRenderer(
                "[{1}]({0})]",
                (x, assemblyName) => { var xx = XmlToMarkdown.ExtractNameAndBody("cref", x, assemblyName); xx[0] = xx[0].ToLower(); return xx; }
            ),
            ["firstparam"] = new TagRenderer(
                "|Name | Description |\n|-----|------|\n|{0}: |{1}|\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBody("name", x, assemblyName)
            ),
            ["typeparam"] = new TagRenderer(
                "|{0}: |{1}|\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBody("name", x, assemblyName)
            ),
            ["param"] = new TagRenderer(
                "|{0}: |{1}|\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBody("name", x, assemblyName)
            ),
            ["paramref"] = new TagRenderer(
                "`{0}`",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBody("name", x, assemblyName)
            ),
            ["exception"] = new TagRenderer(
                "[[{0}|{0}]]: {1}\n\n",
                (x, assemblyName) => XmlToMarkdown.ExtractNameAndBody("cref", x, assemblyName)
            ),
            ["returns"] = new TagRenderer(
                "**Returns**: {0}\n\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["c"] = new TagRenderer(
                " `{0}` ",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            // Modern XML doc tags
            ["inheritdoc"] = new TagRenderer(
                "*Inherits documentation from base.*\n\n",
                (x, assemblyName) => Array.Empty<string>()
            ),
            ["see"] = new TagRenderer(
                "[`{0}`]({0})",
                (x, assemblyName) => new[] { x.Attribute("cref")?.Value?.ExtractLastPart() ?? x.Attribute("href")?.Value ?? x.Value ?? "" }
            ),
            ["seeLangword"] = new TagRenderer(
                "`{0}`",
                (x, assemblyName) => new[] { x.Attribute("langword")?.Value ?? "" }
            ),
            ["seealso"] = new TagRenderer(
                "**See also**: [`{0}`]({0})\n\n",
                (x, assemblyName) => new[] { x.Attribute("cref")?.Value?.ExtractLastPart() ?? x.Attribute("href")?.Value ?? x.Value ?? "" }
            ),
            ["list"] = new TagRenderer(
                "{0}\n\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["item"] = new TagRenderer(
                "- {0}\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["term"] = new TagRenderer(
                "**{0}**: ",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["description"] = new TagRenderer(
                "{0}",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["listheader"] = new TagRenderer(
                "{0}\n",
                (x, assemblyName) => new[] { x.Nodes().ToMarkDown(assemblyName) }
            ),
            ["typeparamref"] = new TagRenderer(
                "`{0}`",
                (x, assemblyName) => new[] { x.Attribute("name")?.Value ?? "" }
            ),
            ["langword"] = new TagRenderer(
                "`{0}`",
                (x, assemblyName) => new[] { x.Attribute("cref")?.Value ?? x.Value ?? "" }
            ),
            ["none"] = new TagRenderer(
                "",
                (x, assemblyName) => Array.Empty<string>()
            ),
        };
    }
}
