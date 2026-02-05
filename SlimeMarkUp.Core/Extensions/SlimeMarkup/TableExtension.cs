using System.Collections.Generic;
using System.Linq;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// A markup extension that parses Markdown-style tables and converts them to HTML &lt;table&gt;, &lt;tr&gt;, and &lt;td&gt; elements.
    /// Supports rows starting with "|" and ignores separator rows consisting only of "-" or "=" characters.
    /// </summary>
    public class TableExtension : IBlockMarkupExtension
    {
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
        /// Gets the number of table rows processed. 
        /// Currently always returns 0 because counting is not implemented.
        /// </summary>
        public int Count { get; }
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
        /// Determines if a line of text can be parsed as a table by this extension.
        /// Lines starting with "|" are considered table rows.
        /// </summary>
        /// <param name="line">The line of text to check.</param>
        /// <returns><c>true</c> if the line starts with a Markdown-style table marker;
        /// otherwise, <c>false</c>.</returns>
        public bool CanParse(string line) => line.TrimStart().StartsWith("|");
        /// <summary>
        /// Parses a single line as a table.
        /// Currently not implemented; always returns <c>null</c>.
        /// </summary>
        /// <param name="line">The line of text to parse.</param>
        /// <returns><c>null</c></returns>
        public MarkupElement? Parse(string line) => null; // Δεν χρησιμοποιείται
        /// <summary>
        /// Parses a block of lines from a queue as a Markdown-style table.
        /// Each line starting with "|" is treated as a table row, with cells separated by "|".
        /// Rows consisting only of "-" or "=" are ignored as separators.
        /// </summary>
        /// <param name="lines">A queue of lines to parse.</param>
        /// <returns>
        /// An enumerable containing a single <see cref="MarkupElement"/> representing the table,
        /// with each row wrapped in &lt;tr&gt; and each cell in &lt;td&gt;.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var rows = new List<List<string>>();

            while (lines.Count > 0 && lines.Peek().TrimStart().StartsWith("|"))
            {
                var line = lines.Dequeue().Trim();
                // Αγνόησε γραμμές separator (μόνο - ή =)
                var cells = line.Trim('|').Split('|').Select(c => c.Trim()).ToList();
                if (cells.All(c => c.All(ch => ch == '-' || ch == '=')))
                    continue;
                rows.Add(cells);
            }

            var htmlRows = rows.Select(row =>
                "<tr>" + string.Join("", row.Select(cell => $"<td>{cell}</td>")) + "</tr>"
            );

            var html = "<table>" + string.Join("", htmlRows) + "</table>";

            return new[] { new MarkupElement { Tag = "table", Content = html } };
        }
    }
}
