using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// Markup extension that allows including external files into the document using a special comment syntax.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// <!-- include: path/to/file.md -->
    /// </code>
    /// <para>
    /// The path is resolved to an absolute path. If the file exists, its content is read and returned
    /// as a <c>raw</c> block inside <see cref="MarkupElement.Content"/>.
    /// Otherwise, an HTML comment indicating an error is generated.
    /// </para>
    /// <para>
    /// This extension has priority and is processed before other extensions.
    /// </para>
    /// </remarks>
    [Obsolete]
    public class IncludeExtension : IBlockMarkupExtension
    {

        
        public int Order
        {
            get
            {
                return 1;
            }
        }
        /// <summary>
        /// Indicates whether this extension has priority over others.
        /// </summary>
        /// <returns>
        /// Always returns <c>true</c>.
        /// </returns>
        public bool Priority()
        {
            return true;
        }
        private static readonly Regex IncludeRegex = new(@"<!--\s*include:\s*(.+?)\s*-->", 
            RegexOptions.Compiled);
        static int count=0;
        /// <summary>
        /// Determines whether the specified line contains an include directive.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line matches <c>&lt;!-- include: ... --&gt;</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line)
        {
            return IncludeRegex.IsMatch(line);
        }
        /// <summary>
        /// Parses a single line containing an include directive.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// A <see cref="MarkupElement"/> with <c>raw</c> tag containing the file content,
        /// or an HTML comment indicating an error if the file cannot be found or read.
        /// </returns>
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
                  var content = File.ReadAllText(fullPath);


                return new MarkupElement
                {
                    Tag = "raw",
                    Content = "<!-- start of file :" + inputPath + " -->\n" + content + "\n<!-- end of file : " + inputPath
                    + " -->"




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
        /// <summary>
        /// Parses a block containing an include directive from the provided queue of lines.
        /// </summary>
        /// <param name="lines">A queue containing the remaining lines of the document.</param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/> for the included content,
        /// or <c>null</c> if no match is found.
        /// </returns>
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
        /// <summary>
        /// Gets the number of included files processed by this extension.
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// Gets a value indicating whether the block should be processed immediately.
        /// </summary>
        public bool IsToBeProccessed
        { get { return true; } }
    }
}
