using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// Markup extension that parses horizontal rules in Markdown.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// ---
    /// ***
    /// ___
    /// </code>
    /// <para>
    /// Lines containing 3 or more consecutive <c>-</c>, <c>*</c>, or <c>_</c>
    /// (optionally separated by spaces) are converted to <c>&lt;hr/&gt;</c>.
    /// </para>
    /// </remarks>
    public class BlockquoteExtension : IBlockMarkupExtension
    {
        private static readonly Regex  hrRegex = new Regex(
    @"^(?: {0,3})(?:(?:\* {0,2}){3,}|(?:- {0,2}){3,}|(?:_ {0,2}){3,})$",
    RegexOptions.Compiled
);
        /// <summary>
        /// Gets the number of parsed horizontal rules.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Determines whether the specified line can be parsed as a horizontal rule.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line matches a horizontal rule pattern; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line)
        {
            return hrRegex.IsMatch(line);
        }
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
        /// Parses a single line and converts it to a horizontal rule element.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// A <see cref="MarkupElement"/> representing the horizontal rule.
        /// </returns>
        public MarkupElement? Parse(string line)
        {

            int tc =hrRegex.Matches(line).Count;
            return new MarkupElement
            { 
                Tag = "HorrizontalLine",
                Content = "<hr/>"

            };
        }
        /// <summary>
        /// Parses a block containing a horizontal rule from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/> for the horizontal rule,
        /// or <c>null</c> if no match is found.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0)
            { return null; }
            var line = lines.Dequeue();
            if (!CanParse(line))
                return null;
            var element = Parse(line);
            return element != null ? new[] { element } : null;

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
