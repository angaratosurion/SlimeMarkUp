namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// Markup extension that parses fenced code blocks using triple backticks.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// ```
    /// Console.WriteLine("Hello, world!");
    /// ```
    /// </code>
    /// <para>
    /// All content between the opening and closing fences is preserved
    /// verbatim and is not processed by other markup extensions.
    /// </para>
    /// </remarks>
    public class CodeBlockExtension : IBlockMarkupExtension
    {
        /// <summary>
        /// Gets the number of parsed code block elements.
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Determines whether the specified line can be parsed
        /// as the start of a fenced code block.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line equals <c>```</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line) => line.Trim() == "```";
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
        /// Always returns <c>null</c>, since code blocks are parsed as multi-line blocks.
        /// </returns>
        public MarkupElement? Parse(string line) => null;
        /// <summary>
        /// Parses a fenced code block from the provided queue of lines.
        /// </summary>
        /// <param name="lines">
        /// A queue containing the remaining lines of the document.
        /// </param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/>
        /// with the <c>pre</c> tag and the raw code as content.
        /// </returns>
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            lines.Dequeue(); // remove ```
            var codeLines = new List<string>();
            while (lines.Count > 0 && lines.Peek().Trim() != "```")
                codeLines.Add(lines.Dequeue());
            if (lines.Count > 0) lines.Dequeue(); // remove ending ```

            var code = string.Join("\n", codeLines);
            return new[] { new MarkupElement { Tag = "pre", Content = code } };
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
    }
}
 
