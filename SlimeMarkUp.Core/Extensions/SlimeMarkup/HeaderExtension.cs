using System.Collections.Generic;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// Markup extension that parses Markdown headers using hash (#) symbols.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// # Header 1
    /// ## Header 2
    /// ### Header 3
    /// </code>
    /// <para>
    /// The number of leading <c>#</c> characters determines the header level.
    /// Any leading or trailing whitespace is trimmed from the header content.
    /// </para>
    /// </remarks>
    public class HeaderExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of parsed header elements.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Determines whether the specified line can be parsed as a header.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line starts with one or more <c>#</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line) => line.TrimStart().StartsWith("#");
        /// <summary>
        /// Gets a value indicating whether the block should be
        /// processed immediately.
        /// </summary>
        public bool IsToBeProccessed
        { get { return false; } }
        /// <summary>
        /// Parses a single header line and returns a corresponding <see cref="MarkupElement"/>.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// A <see cref="MarkupElement"/> with <c>h1</c>.. <c>h6</c> tag and the header content.
        /// </returns>
        public MarkupElement? Parse(string line)
        {
            int level = 0;
            while (level < line.Length && line[level] == '#') level++;

            var content = line.Substring(level).Trim();

            return new MarkupElement
            {
                Tag = $"h{level}",
                Content = content
            };
        }
        /// <summary>
        /// Parses a header block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/> representing the header.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0) return null;
            var line = lines.Dequeue();
            return new[] { Parse(line) };
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
    } }
