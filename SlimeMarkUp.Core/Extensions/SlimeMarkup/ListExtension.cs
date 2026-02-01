using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class ListExtension : IBlockMarkupExtension
    {
        public int Count { get; }
        public bool IsToBeProccessed
        { get { return false; } }
        public int Order
        {
            get
            {
                return 2;
            }
        }
        public bool Priority()
        {
            return false;
        }
        public bool CanParse(string line) => line.TrimStart().StartsWith("- ");

        public MarkupElement? Parse(string line) => null;

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var items = new List<string>();

            while (lines.Count > 0)
            {
                var line = lines.Peek();
                if (string.IsNullOrWhiteSpace(line)) break;

                if (line.TrimStart().StartsWith("- "))
                {
                    items.Add(lines.Dequeue().TrimStart().Substring(2));
                }
                else
                {
                    break;
                }
            }

            if (items.Count == 0) return null;

            var html = string.Join("", items.Select(i => $"<li>{i}</li>"));
            return new[] { new MarkupElement { Tag = "ul", Content = html } };
        }
    }
}
