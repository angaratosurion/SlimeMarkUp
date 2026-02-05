using System.Collections.Generic;
using System.Linq;

namespace SlimeMarkUp.Core.Extensions.ObsidianMarkup
{
    /// <summary>
    /// Markup extension that parses Obsidian-style Markdown tables.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// | Name | Age | City |
    /// | ---- | --- | ---- |
    /// | John | 30  | Rome |
    /// | Anna | 25  | Paris |
    /// </code>
    /// <para>
    /// Lines starting with <c>|</c> are treated as table rows.
    /// Separator rows containing only <c>-</c> or <c>=</c> characters
    /// are ignored.
    /// </para>
    /// </remarks>
    public class ObsidianTableExtension : IBlockMarkupExtension
    {
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
        /// Gets the number of parsed table elements.
        /// </summary>
        public int Count { get; }
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
        /// as the start of a table.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line starts with <c>|</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line) => line.TrimStart().StartsWith("|");
        /// <summary>
        /// Parses a single line.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// Always returns <c>null</c>, since tables are parsed as block elements.
        /// </returns>
        public MarkupElement? Parse(string line) => null; // Δεν χρησιμοποιείται
        /// <summary>
        /// Parses a table block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/>
        /// with the <c>table</c> tag and its rows as content.
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
