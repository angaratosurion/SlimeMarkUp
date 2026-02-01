using SlimeMarkUp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core
{
    public class IncludeTagHandler //: IBlockMarkupExtension
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

        public string Parse(string line)
        {
            string ap = "";
            var match = IncludeRegex.Match(line);
            if (!match.Success)
                return null;
             
            var inputPath = match.Groups[1].Value.Trim();
            string fullPath = Path.GetFullPath(inputPath);

            if (!File.Exists(fullPath))
            {
                //return new MarkupElement
                //{
                //    Tag = "p",
                ap  = $"<!-- ERROR: File '{inputPath}' not found -->";
                 
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
                  var content = File.ReadAllText(fullPath);


                ap = "<!-- start of file :" + inputPath + " -->\n" + content + "\n<!-- end of file : " + inputPath
                    + " -->";

                return ap;


                
            }
            catch (Exception ex)
            {
                return $"<!-- ERROR: Could not read '{inputPath}': {ex.Message} -->";
                
            }
        }

        public string ParseBlock(Queue<string> lines)
        {
             if ( lines.Count==0)
            { return null; }
            var line = lines.ToList()[0];
            //if (!CanParse(line))
            //    return null;

            var element = Parse(line);
            return element;
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsToBeProccessed
        { get { return true; } }
    }
}
