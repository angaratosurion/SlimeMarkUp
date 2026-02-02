using System.Collections.Generic;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// Provides an extension for parsing blockquote-style horizontal rules in markup content.
    /// </summary>
    /// <remarks>Implements the <see cref="IBlockMarkupExtension"/> interface to support custom block-level
    /// markup parsing. This extension identifies lines starting with a blockquote marker ('> ') and processes them as
    /// horizontal rule elements. The extension can be used to customize how blockquotes or similar structures are
    /// handled during markup parsing.</remarks>
    public class HorizontalRuleExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Determines whether the specified line represents a quoted block by checking if it begins with a greater-than
        /// sign followed by a space.
        /// </summary>
        /// <param name="line">The line of text to evaluate. Leading whitespace is ignored when determining if the line is a quoted block.</param>
        /// <returns>true if the trimmed line starts with "> "; otherwise, false.</returns>
        public bool CanParse(string line) => line.TrimStart().StartsWith("> ");
        /// <summary>
        /// Gets a value indicating whether the item is marked to be processed.
        /// </summary>
        public bool IsToBeProccessed
        { get { return false; } }
        /// <summary>
        /// Gets the order value associated with this instance.
        /// </summary>
        public int Order
        {
            get
            {
                return 2;
            }
        }
      
        public MarkupElement? Parse(string line) => null;
        
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
        
        public bool Priority()
        {
            return false;
        }
    }
}
