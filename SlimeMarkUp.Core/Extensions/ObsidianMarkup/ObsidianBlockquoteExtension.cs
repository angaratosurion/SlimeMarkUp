using System.Collections.Generic;

namespace SlimeMarkUp.Core.Extensions.ObsidianMarkup
{
    /// <summary>
    /// Markup extension that parses Obsidian-style blockquotes.
    /// </summary>
    /// <remarks>
    /// A blockquote is defined as one or more consecutive lines
    /// starting with the <c>&gt; </c> prefix.
    /// </remarks>
    public class ObsidianBlockquoteExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of parsed blockquote elements.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Determines whether the specified line can be parsed
        /// as the start of a blockquote.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line starts with <c>&gt; </c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line) => line.TrimStart().StartsWith("> ");
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
        /// Parses a single line.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// Always returns <c>null</c>, since blockquotes are parsed as blocks.
        /// </returns>
        public MarkupElement? Parse(string line) => null;
        /// <summary>
        /// Parses a blockquote block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/>
        /// with the <c>blockquote</c> tag.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var contentLines = new List<string>();

            while (lines.Count > 0 && lines.Peek().TrimStart().StartsWith("> "))
            {
                var line = lines.Dequeue();
                contentLines.Add(line.TrimStart().Substring(2));
            }

            var content = string.Join(" ", contentLines);

            return new[]
            {
                new MarkupElement
                {
                    Tag = "blockquote",
                    Content = content
                }
            };
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
    }
}
