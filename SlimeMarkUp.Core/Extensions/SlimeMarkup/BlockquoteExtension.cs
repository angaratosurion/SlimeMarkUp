using System.Collections.Generic;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class HorizontalRuleExtension : IBlockMarkupExtension
    {
        public int Count { get; }
        public bool CanParse(string line) => line.TrimStart().StartsWith("> ");
        public bool IsToBeProccessed
        { get { return false; } }
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
