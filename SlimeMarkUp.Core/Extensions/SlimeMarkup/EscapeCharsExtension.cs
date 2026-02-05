using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// Markup extension that handles escaped or special character lines.
    /// </summary>
    /// <remarks>
    /// Currently, this extension is a placeholder for lines that should
    /// not be processed by other extensions, such as escaped numbered lists
    /// or custom escape sequences.
    /// </remarks>
    public class EscapeCharsExtension : IBlockMarkupExtension
    {
        private static readonly Regex escaperegex =
           new Regex(@"^(?:\s{0,3})\d+[.)]\s+", RegexOptions.Compiled);
        /// <summary>
        /// Gets the number of parsed escape elements.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Determines whether the specified line can be parsed by this extension.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// Always returns <c>false</c> since this extension only processes blocks.
        /// </returns>
        public bool CanParse(string line) { return false; }
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
        /// Currently always returns <c>null</c>.
        /// </returns>
        public MarkupElement? Parse(string line)
        {
             

           
            
                return null;
             
        }
        /// <summary>
        /// Parses a block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing the result of <see cref="Parse(string)"/>.
        /// Currently always <c>null</c>.
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
