using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class BlockquoteExtension : IBlockMarkupExtension
    {
        private static readonly Regex  hrRegex = new Regex(
    @"^(?: {0,3})(?:(?:\* {0,2}){3,}|(?:- {0,2}){3,}|(?:_ {0,2}){3,})$",
    RegexOptions.Multiline
);

        public int Count { get; }
        public bool CanParse(string line)
        {
            return hrRegex.IsMatch(line);
        }
        public bool IsToBeProccessed
        { get { return false; } }
        public int Order
        {
            get
            {
                return 2;
            }
        }

        public MarkupElement? Parse(string line)
        {

            int tc =hrRegex.Matches(line).Count;
            return new MarkupElement
            {
                //Tag = "raw",
                //Content = "<!-- start of file :"+ inputPath+" -->" + content+ "<!-- end of file : "+ inputPath 
                //+" -->"
                Tag = "HorrizontalLine",
                Content = "<hr/>"

            };
        }
        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0)
            { return null; }
            var line = lines.Dequeue();
            if (!CanParse(line))
                return null;
            var element = Parse(line);
            return element != null ? new[] { element } : null;

        }

        public bool Priority()
        {
            return false;
        }
    }
}
