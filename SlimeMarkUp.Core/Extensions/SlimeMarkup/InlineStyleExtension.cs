using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class InlineStyleExtension : IBlockMarkupExtension
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
        public bool CanParse(string line) =>
            (line.Contains("**") || line.Contains("*") || line.Contains("~") 
            || line.Contains("+")) &&(!line.StartsWith("\\"));

        public MarkupElement? Parse(string line)
        {
            var content = ApplyInlineStyles(line);
            return new MarkupElement
            {
                Tag = "p",
                Content = content
            };
        }

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0)
                return null;

            var line = lines.Dequeue();
            if (!CanParse(line))
                return null;

            var content = ApplyInlineStyles(line);
            return new[]
            {
                new MarkupElement
                {
                    Tag = "p",
                    Content = content
                }
            };
        }

        private string ApplyInlineStyles(string input)
        {
            var result = input;
            result = Regex.Replace(result, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            result = Regex.Replace(result, @"\*(.+?)\*", "<em>$1</em>");
             result = Regex.Replace(result, @"\~\~(.+?)\~\~", "<s>$1</s>");
            result = Regex.Replace(result, @"\+\+(.+?)\+\+", "<u>$1</u>");
            return result;
        }
    }
}
