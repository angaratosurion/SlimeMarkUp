using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// A markup extension that parses Markdown-style links and converts them to HTML anchor (&lt;a&gt;) tags.
    /// Supports optional inline attributes in the format {key=value key2=value2}.
    /// </summary>
    public class LinkExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of links processed. 
        /// Currently always returns 0 because counting is not implemented.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Indicates whether this extension should be processed. 
        /// Always returns <c>false</c>.
        /// </summary>
        public bool IsToBeProccessed
        { get { return false; } }
        /// <summary>
        /// Gets the order in which this extension should be applied relative to others.
        /// Lower numbers are processed earlier. This extension has an order of 2.
        /// </summary>
        public int Order
        {
            get
            {
                return 2;
            }
        }
        /// <summary>
        /// Determines whether this extension has priority over others.
        /// Always returns <c>false</c>.
        /// </summary>
        /// <returns><c>false</c></returns>
        public bool Priority()
        {
            return false;
        }
        /// <summary>
        /// Determines if a line of text can be parsed as a link by this extension.
        /// Lines matching the Markdown link pattern [text](url) are parseable,
        /// unless they start with a backslash (\), which escapes the syntax.
        /// </summary>
        /// <param name="line">The line of text to check.</param>
        /// <returns><c>true</c> if the line contains a Markdown-style link; otherwise, <c>false</c>.</returns>
        public bool CanParse(string line) => (Regex.IsMatch(line, @"\[(.*?)\]\((.*?)\)"))
            && (!line.StartsWith("\\"));
        /// <summary>
        /// Parses a single line as a link.
        /// Currently not implemented; always returns <c>null</c>.
        /// </summary>
        /// <param name="line">The line of text to parse.</param>
        /// <returns><c>null</c></returns>
        public MarkupElement? Parse(string line) => null;
        /// <summary>
        /// Parses a block of lines from a queue, extracting Markdown-style links and converting them 
        /// to HTML &lt;a&gt; tags.
        /// Optional inline attributes in the format {key=value key2=value2} are appended to the anchor tag.
        /// </summary>
        /// <param name="lines">A queue of lines to parse.</param>
        /// <returns>An enumerable containing a single <see cref="MarkupElement"/> with the 
        /// generated HTML anchor tag.</returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var line = lines.Dequeue();

            var textMatch = Regex.Match(line, @"\[(.*?)\]");
            var hrefMatch = Regex.Match(line, @"\((.*?)\)");
            var attrMatch = Regex.Match(line, @"\{(.*?)\}");

            var text = textMatch.Success ? textMatch.Groups[1].Value : "";
            var href = hrefMatch.Success ? hrefMatch.Groups[1].Value : "";

            var attributes = $"href=\"{href}\"";

            if (attrMatch.Success)
            {
                var attrParts = attrMatch.Groups[1].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in attrParts)
                {
                    var kv = part.Split('=');
                    if (kv.Length == 2)
                    {
                        var key = kv[0];
                        var value = kv[1];
                        attributes += $" {key}=\"{value}\"";
                    }
                }
            }

            var html = $"<a {attributes}>{text}</a>";

            return new[] { new MarkupElement { Tag = "a", Content = html } };
        }
    }
}
