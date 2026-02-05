using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeMarkUp.Core.Extensions.ObsidianMarkup
{
    /// <summary>
    /// Markup extension that parses unordered lists using dash-prefixed items.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// - First item
    /// - Second item
    /// - Third item
    /// </code>
    /// <para>
    /// Each list item must start with <c>- </c>. Nested lists
    /// and alternative list markers are not supported.
    /// </para>
    /// </remarks>
    public class ObsidianListExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of parsed list elements.
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
        /// Determines whether the specified line can be parsed
        /// as the start of an unordered list.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line starts with <c>- </c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line) => line.TrimStart().StartsWith("- ");

        /// <summary>
        /// Parses a single line.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// This method is not supported for list parsing.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Thrown because lists are parsed only as block elements.
        /// </exception>
        public MarkupElement? Parse(string line) =>
            throw new NotImplementedException("ListExtension only supports block parsing.");
        /// <summary>
        /// Parses an unordered list block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/>
        /// with the <c>ul</c> tag and its list items as content,
        /// or <c>null</c> if no list items are found.
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
