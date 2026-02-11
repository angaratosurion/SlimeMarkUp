using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
     
    /// <summary>
    /// A block markup extension that suppresses all HTML content
    /// except HTML comments that start with <c>&lt;!--include:</c>.
    /// </summary>
    /// <remarks>
    /// This extension is intended to prevent raw HTML from being processed
    /// by the markup pipeline while still allowing controlled include directives.
    ///
    /// Any line that appears to be HTML (i.e. starts with '&lt;' and ends with '&gt;')
    /// will be ignored unless it starts with:
    ///
    ///     <!--include:
    ///
    /// Include directives are returned as <see cref="MarkupElement"/> instances
    /// with Tag = "include".
    ///
    /// All other HTML lines are consumed and discarded.
    /// </remarks>
    public class HtmlIgnoreExceptIncludeExtension : IBlockMarkupExtension
    {
        private int _count;

        /// <summary>
        /// Gets the execution order of the extension within the pipeline.
        /// Lower values typically execute earlier.
        /// </summary>
        public int Order => 0;

        /// <summary>
        /// Gets the number of elements successfully produced by this extension.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Gets a value indicating whether this extension participates
        /// in processing.
        /// </summary>
        public bool IsToBeProccessed => true;

        /// <summary>
        /// Indicates that this extension has priority over non-priority extensions.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> to ensure HTML suppression occurs early in the pipeline.
        /// </returns>
        public bool Priority() => true;

        /// <summary>
        /// Determines whether the specified line represents HTML content
        /// or a supported include directive.
        /// </summary>
        /// <param name="line">The line to evaluate.</param>
        /// <returns>
        /// <see langword="true"/> if the line appears to be HTML or an include directive;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanParse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;

            var trimmed = line.TrimStart();

            // Allow include directives
            if (trimmed.StartsWith("<!--include:", StringComparison.OrdinalIgnoreCase))
                return true;

            // Detect probable HTML
            if (trimmed.StartsWith("<") && trimmed.EndsWith(">"))
                return true;

            return false;
        }

        /// <summary>
        /// Parses a single line of HTML or include directive.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <returns>
        /// A <see cref="MarkupElement"/> for include directives;
        /// otherwise <see langword="null"/> for ignored HTML.
        /// </returns>
        public MarkupElement? Parse(string line)
        {
            var trimmed = line.TrimStart();

            if (trimmed.StartsWith("<!--include:", StringComparison.OrdinalIgnoreCase))
            {
                _count++;

                return new MarkupElement
                {
                    Tag = "include",
                    Content = trimmed
                };
            }

            // Suppress all other HTML
            return null;
        }

        /// <summary>
        /// Parses and consumes a block of lines from the input queue.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining document lines.
        /// </param>
        /// <returns>
        /// A collection containing a single include element if detected;
        /// otherwise an empty collection if HTML was suppressed;
        /// or <see langword="null"/> if the extension cannot process the block.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0)
                return null;

            var line = lines.Peek();

            if (!CanParse(line))
                return null;

            lines.Dequeue();

            var element = Parse(line);

            if (element == null)
                return Enumerable.Empty<MarkupElement>();

            return new[] { element };
        }
    }


}
