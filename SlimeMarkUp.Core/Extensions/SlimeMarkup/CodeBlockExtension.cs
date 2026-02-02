namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
   
    public class CodeBlockExtension : IBlockMarkupExtension
    {
        public int Count { get; }
        public bool CanParse(string line) => line.Trim() == "```";
        public bool IsToBeProccessed
        { get { return false; } }
        public MarkupElement? Parse(string line) => null;

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
    
 
    public bool Priority()
        {
            return false;
        }
        public int Order
        {
            get
            {
                return 2;
            }
        }
    }
}
 
