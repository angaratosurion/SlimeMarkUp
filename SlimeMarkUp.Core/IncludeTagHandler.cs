using SlimeMarkUp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core
{
    /// <summary>
    /// Provides functionality to detect and process custom include tags within text lines, enabling the inclusion of
    /// external file content based on a specific markup pattern.
    /// </summary>
    /// <remarks>The include tag handler identifies lines containing an include directive in the format '<!--
    /// include: path -->' and replaces them with the contents of the specified file. This class is typically used in
    /// scenarios where dynamic content injection from external files is required, such as in markup or documentation
    /// processing workflows. The handler maintains a count of processed include tags and exposes properties to control
    /// parsing order and processing behavior.</remarks>
    public class IncludeTagHandler  
    {
        /// <summary>
        /// Gets the order value used to determine the relative position or priority of this item.
        /// </summary>
        
        public int Order
        {
            get
            {
                return 1;
            }
        }
        /// <summary>
        /// Indicates whether the current item has priority status.
        /// </summary>
        /// <returns><see langword="true"/> if the item is prioritized; otherwise, <see langword="false"/>.</returns>
        public bool Priority()
        {
            return true;
        }
        private static readonly Regex IncludeRegex = new(@"<!--\s*include:\s*(.+?)\s*-->", 
            RegexOptions.Compiled);
        static int count=0;
        /// <summary>
        /// Determines whether the specified line matches the criteria for parsing.
        /// </summary>
        /// <param name="line">The line of text to evaluate against the parsing criteria. Cannot be null.</param>
        /// <returns>true if the line matches the criteria and can be parsed; otherwise, false.</returns>
        public bool CanParse(string line)
        {
            return IncludeRegex.IsMatch(line);
        }
        /// <summary>
        /// Parses the specified line to extract an include directive and returns the contents of the referenced file,
        /// wrapped with comment markers.
        /// </summary>
        /// <remarks>If the specified file does not exist or cannot be read, the returned string contains
        /// an HTML comment describing the error. The method does not throw exceptions for missing or unreadable files;
        /// instead, it returns an error message in the output.</remarks>
        /// <param name="line">The line of text to parse for an include directive. Must contain a valid include pattern; otherwise, the
        /// method returns null.</param>
        /// <returns>A string containing the contents of the included file, wrapped with start and end comment markers. Returns
        /// null if the line does not contain a valid include directive. Returns an error comment if the file is not
        /// found or cannot be read.</returns>
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
        /// <summary>
        /// Parses the first line from the specified queue and returns its processed representation.
        /// </summary>
        /// <param name="lines">A queue of strings containing lines to be parsed. Must not be empty.</param>
        /// <returns>A string representing the parsed result of the first line in the queue, or null if the queue is empty.</returns>
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
