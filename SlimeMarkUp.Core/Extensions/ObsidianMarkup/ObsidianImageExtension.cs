using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.ObsidianMarkup
{
    /// <summary>
    /// Markup extension that parses Obsidian-style Markdown image syntax.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// ![alt text](image.png){width=300 height=200}
    /// </code>
    /// <para>
    /// The extension extracts the image source, alternative text,
    /// and optional attribute definitions, converting them into
    /// an <c>&lt;img /&gt;</c> markup element.
    /// </para>
    /// </remarks>
    public class ObsidianImageExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of parsed image elements.
        /// </summary>
        public int Count { get; }


        /// <summary>
        /// Determines whether the specified line can be parsed as an image.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line starts with <c>![</c>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        public bool CanParse(string line) => line.StartsWith("![");
        /// <summary>
        /// Gets a value indicating whether the block should be
        /// processed immediately.
        /// </summary>
        public bool IsToBeProccessed
        { get { return false; } }
        /// <summary>
        /// Parses a single line.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// Always returns <c>null</c>, since images are parsed as block elements.
        /// </returns>
        public MarkupElement? Parse(string line) => null; // Δεν χρησιμοποιείται

        /// <summary>
        /// Parses an image block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/>
        /// representing an <c>img</c> element.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var line = lines.Dequeue();

            var altMatch = Regex.Match(line, @"!\[(.*?)\]");
            var srcMatch = Regex.Match(line, @"\((.*?)\)");
            var attrMatch = Regex.Match(line, @"\{(.*?)\}");

            var alt = altMatch.Success ? altMatch.Groups[1].Value : "";
            var src = srcMatch.Success ? srcMatch.Groups[1].Value : "";

            var attributes = $"src=\"{src}\" alt=\"{alt}\"";

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

            var html = $"<img {attributes} />";

            return new[] { new MarkupElement { Tag = "img", Content = html } };
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
        /// Gets the execution order of the extension.
        /// </summary>
        public int Order
        {
            get
            {
                return 2;
            }
        }
    }
}
