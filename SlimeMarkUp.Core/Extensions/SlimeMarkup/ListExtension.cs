using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// A markup extension that parses Markdown-style unordered lists and converts them
    /// to HTML &lt;ul&gt; and &lt;li&gt; tags.
    /// Supports list items starting with "- " and stops parsing at empty lines or non-list lines.
    /// </summary>
    public class ListExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of list items processed. 
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
        /// Determines if a line of text can be parsed as a list by this extension.
        /// Lines starting with "- " (after trimming leading whitespace) are considered list items.
        /// </summary>
        /// <param name="line">The line of text to check.</param>
        /// <returns><c>true</c> if the line starts with a Markdown-style 
        /// list marker; otherwise, <c>false</c>.</returns>
        public bool CanParse(string line) => line.TrimStart().StartsWith("- ");
        /// <summary>
        /// Parses a single line as a list item.
        /// Currently not implemented; always returns <c>null</c>.
        /// </summary>
        /// <param name="line">The line of text to parse.</param>
        /// <returns><c>null</c></returns>
        public MarkupElement? Parse(string line) => null;
        /// <summary>
        /// Parses a block of lines from a queue, extracting consecutive Markdown-style list items.
        /// Each line starting with "- " becomes a &lt;li&gt; element, wrapped in a &lt;ul&gt; tag.
        /// Parsing stops when an empty line or a non-list line is encountered.
        /// </summary>
        /// <param name="lines">A queue of lines to parse.</param>
        /// <returns>
        /// An enumerable containing a single <see cref="MarkupElement"/> representing the unordered list,
        /// or <c>null</c> if no list items were found.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var items = new List<string>();

            while (lines.Count > 0)
            {
                var line = lines.Peek();
                if (string.IsNullOrWhiteSpace(line)) break;

                if (line.TrimStart().StartsWith("- "))
                {
                    items.Add(lines.Dequeue().TrimStart().Substring(2));
                }
                else
                {
                    break;
                }
            }

            if (items.Count == 0) return null;

            var html = string.Join("", items.Select(i => $"<li>{i}</li>"));
            return new[] { new MarkupElement { Tag = "ul", Content = html } };
        }
    }
}
