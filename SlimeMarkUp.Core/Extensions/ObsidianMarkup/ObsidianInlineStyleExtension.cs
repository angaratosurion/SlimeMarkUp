using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.ObsidianMarkup
{
    /// <summary>
    /// Markup extension that parses inline text styles such as bold and italic.
    /// </summary>
    /// <remarks>
    /// Supported inline syntax:
    /// <list type="bullet">
    ///   <item><description><c>**bold**</c> → <c>&lt;strong&gt;</c></description></item>
    ///   <item><description><c>*italic*</c> → <c>&lt;em&gt;</c></description></item>
    /// </list>
    /// <para>
    /// Inline styles are applied using regular expressions and
    /// are not nested or escaped.
    /// </para>
    /// </remarks>
    public class ObsidianInlineStyleExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of parsed inline style elements.
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
    /// supported inline style markers.
    /// </summary>
    /// <param name="line">The input line.</param>
    /// <returns>
    /// <c>true</c> if the line contains <c>**</c> or <c>*</c>;
    /// otherwise, <c>false</c>.
    /// </returns>
        public bool CanParse(string line) =>
            line.Contains("**") || line.Contains("*");

        /// <summary>
        /// Parses a single line and applies inline style transformations.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// A paragraph <see cref="MarkupElement"/> with inline styles applied.
        /// </returns>
        public MarkupElement? Parse(string line)
        {
            var content = ApplyInlineStyles(line);
            return new MarkupElement
            {
                Tag = "p",
                Content = content
            };
        }

        /// <summary>
        /// Parses a block containing inline styles from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single paragraph
        /// <see cref="MarkupElement"/> with inline styles applied,
        /// or <c>null</c> if the line does not contain inline syntax.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0)
                return null;

            var line = lines.Dequeue();
            if (!CanParse(line))
                return null;

            var content = ApplyInlineStyles(line);
            return new[]
            {
                new MarkupElement
                {
                    Tag = "p",
                    Content = content
                }
            };
        }
        /// <summary>
        /// Applies inline style transformations to the specified input string.
        /// </summary>
        /// <param name="input">The raw input text.</param>
        /// <returns>
        /// A string with Markdown-style inline markers replaced by HTML tags.
        /// </returns>
        private string ApplyInlineStyles(string input)
        {
            var result = input;
            result = Regex.Replace(result, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            result = Regex.Replace(result, @"\*(.+?)\*", "<em>$1</em>");
            return result;
        }
    }
}
