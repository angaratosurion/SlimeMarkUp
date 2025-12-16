using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class IncludeExtension : IBlockMarkupExtension
    {

        
        public int Order
        {
            get
            {
                return 1;
            }
        }
        public bool Priority()
        {
            return true;
        }
        private static readonly Regex IncludeRegex = new(@"<!--\s*include:\s*(.+?)\s*-->", 
            RegexOptions.Compiled);
        static int count=0;
        public bool CanParse(string line)
        {
            return IncludeRegex.IsMatch(line);
        }

        public MarkupElement? Parse(string line)
        {
            var match = IncludeRegex.Match(line);
            if (!match.Success)
                return null;
             
            var inputPath = match.Groups[1].Value.Trim();
            string fullPath = Path.GetFullPath(inputPath);

            if (!File.Exists(fullPath))
            {
                return new MarkupElement
                {
                    Tag = "p",
                    Content = $"<!-- ERROR: File '{inputPath}' not found -->"
                };
            }

            try
            {
                int tc = IncludeRegex.Matches(line).Count;
                //if( Count()==0)
                //{
                //    count = tc;
                   
                //}
                //else
                //{
                    count += tc;

                //}
              //  var content = File.ReadAllText(fullPath);
                return new MarkupElement
                {
                    //Tag = "raw",
                    //Content = "<!-- start of file :"+ inputPath+" -->" + content+ "<!-- end of file : "+ inputPath 
                    //+" -->"
                    Tag = "iframe",
                    Content = "<iframe src =\""+fullPath + 
                    "\"style=\"  width: 100% ;height:100%;\"/>"

                };
            }
            catch (Exception ex)
            {
                return new MarkupElement
                {
                    Tag = "p",
                    Content = $"<!-- ERROR: Could not read '{inputPath}': {ex.Message} -->"
                };
            }
        }

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
             if ( lines.Count==0)
            { return null; }
            var line = lines.Dequeue();
            if (!CanParse(line))
                return null;

            var element = Parse(line);
            return element != null ? new[] { element } : null;
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsToBeProccessed
        { get { return true; } }
    }
}
