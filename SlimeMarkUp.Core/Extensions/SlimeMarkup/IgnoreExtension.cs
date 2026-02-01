using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class IgnoreExtension : IBlockMarkupExtension
    {
        private static readonly Regex OrderedListRegex =
           new Regex(@"^(?:\s{0,3})\d+[.)]\s+");
        public int Count { get; }
        public bool CanParse(string line) => line.TrimStart().StartsWith("#");
        public bool IsToBeProccessed
        { get { return false; } }
        public MarkupElement? Parse(string line)
        {
            int level = 0;

            var content = line;
            if (OrderedListRegex.IsMatch(line))
            { 

            return new MarkupElement
            {
                Tag = "text",
                Content = content
            };
           }
            else
            {
                return null;
            }
        }

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0) return null;
            var line = lines.Dequeue();
            return new[] { Parse(line) };
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
    } }
