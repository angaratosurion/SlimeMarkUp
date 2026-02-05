using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.ObsidianMarkup
{
    /// <summary>
    /// Markup extension that parses Obsidian-style Markdown links.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// [link text](https://example.com){target=_blank rel=noopener}
    /// </code>
    /// <para>
    /// The extension extracts the link text, URL, and optional
    /// attribute definitions, converting them into an
    /// <c>&lt;a&gt;</c> markup element.
    /// </para>
    /// </remarks>
    public class ObsidianLinkExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of parsed link elements.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Gets a value indicating whether the block should be
        /// processed immediately.
        /// </summary>
        public bool IsToBeProccessed
        { get { return false; } }
        /// <summary>
        /// Gets the execution order of the extension.
        /// </summary>
        public int Order
        {
            get
            {
                return 2;
            }
        }
        /// <summary>
        /// Indicates whether this extension has priority over others.
        /// </summary>
        /// <returns>
        /// Always returns <c>false</c>.
        /// </returns>
        public bool Priority()
        {
            return false;
        }
        /// <summary>
        /// Determines whether the specified line contains
        /// a Markdown-style link.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line matches the pattern
        /// <c>[text](url)</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line) => Regex.IsMatch(line, @"\[(.*?)\]\((.*?)\)");
        /// <summary>
        /// Parses a single line.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// Always returns <c>null</c>, since links are parsed as block elements.
        /// </returns>
        public MarkupElement? Parse(string line) => null;
        /// <summary>
        /// Parses a link block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/>
        /// representing an <c>a</c> (anchor) element.
        /// </returns>

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
