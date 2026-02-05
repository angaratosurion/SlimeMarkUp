using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// A markup extension that parses inline style elements in a line of text and converts them to HTML-like tags.
    /// Supports bold (**text**), italic (*text*), strikethrough (~~text~~), and underline (++text++).
    /// </summary>
    public class InlineStyleExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of inline styles detected or applied. 
        /// Currently always returns 0 because counting is not implemented.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Indicates whether this extension should be processed. 
        /// Always returns <c>false</c> as processing is handled manually.
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
        /// Determines if a line of text can be parsed by this extension.
        /// Lines containing **, *, ~~, or ++ are considered parseable,
        /// unless they start with a backslash (\), which escapes formatting.
        /// </summary>
        /// <param name="line">The input line to check.</param>
        /// <returns><c>true</c> if the line contains inline style syntax and is not
        /// escaped; otherwise, <c>false</c>.</returns>
        public bool CanParse(string line) =>
            (line.Contains("**") || line.Contains("*") || line.Contains("~") 
            || line.Contains("+")) &&(!line.StartsWith("\\"));
        /// <summary>
        /// Parses a single line of text and applies inline styles.
        /// Wraps the resulting content in a <c>p</c> tag.
        /// </summary>
        /// <param name="line">The line of text to parse.</param>
        /// <returns>A <see cref="MarkupElement"/> containing the styled content.</returns>

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
        /// Parses a block of lines from a queue, applying inline styles to the first line if it is parseable.
        /// </summary>
        /// <param name="lines">A queue of lines to parse.</param>
        /// <returns>An enumerable containing a single <see cref="MarkupElement"/> with styled content,
        /// or <c>null</c> if no lines can be parsed.</returns>
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
        /// Applies inline style transformations to the input string.
        /// Converts **text** to <strong>, *text* to <em>, ~~text~~ to <s>, and ++text++ to <u>.
        /// </summary>
        /// <param name="input">The raw input string.</param>
        /// <returns>The string with HTML-like inline tags applied.</returns>
        private string ApplyInlineStyles(string input)
        {
            var result = input;
            result = Regex.Replace(result, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            result = Regex.Replace(result, @"\*(.+?)\*", "<em>$1</em>");
             result = Regex.Replace(result, @"\~\~(.+?)\~\~", "<s>$1</s>");
            result = Regex.Replace(result, @"\+\+(.+?)\+\+", "<u>$1</u>");
            return result;
        }
    }
}
