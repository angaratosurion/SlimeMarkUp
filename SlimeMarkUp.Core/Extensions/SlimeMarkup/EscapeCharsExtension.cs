using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class EscapeCharsExtension : IBlockMarkupExtension
    {
        private static readonly Regex escaperegex =
           new Regex(@"^(?:\s{0,3})\d+[.)]\s+", RegexOptions.Compiled);
        public int Count { get; }
        public bool CanParse(string line) { return false; }
        public bool IsToBeProccessed
        { get { return false; } }
        public MarkupElement? Parse(string line)
        {
             

           
            
                return null;
             
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
