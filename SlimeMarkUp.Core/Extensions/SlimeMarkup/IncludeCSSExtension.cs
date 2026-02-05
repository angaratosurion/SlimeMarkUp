using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    /// <summary>
    /// Markup extension that allows including external CSS files using a special comment syntax.
    /// </summary>
    /// <remarks>
    /// Supported syntax:
    /// <code>
    /// <!-- include style: path/to/file.css -->
    /// </code>
    /// <para>
    /// The path is resolved to an absolute path. If the file exists, a
    /// <c>&lt;link rel="stylesheet"&gt;</c> element is returned.
    /// Otherwise, an HTML comment indicating an error is generated.
    /// </para>
    /// <para>
    /// This extension has priority and is processed before other extensions.
    /// </para>
    /// </remarks>
    public class IncludeCSSExtension : IBlockMarkupExtension
    {

        /// <summary>
        /// Gets the execution order of the extension.
        /// </summary>
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
        private static readonly Regex IncludeRegex = new(@"<!--\s*include style:\s*(.+?)\s*-->", 
            RegexOptions.Compiled);
        static int count=0;
        /// <summary>
        /// Determines whether the specified line contains a CSS include directive.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// <c>true</c> if the line matches <c>&lt;!-- include style: ... --&gt;</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool CanParse(string line)
        {
            return IncludeRegex.IsMatch(line);
        }
        /// <summary>
        /// Parses a single line containing a CSS include directive.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <returns>
        /// A <see cref="MarkupElement"/> with a <c>link</c> tag referencing the CSS file,
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
              //  var content = File.ReadAllText(fullPath);
                return new MarkupElement
                {
                    //Tag = "raw",
                    //Content = "<!-- start of file :"+ inputPath+" -->" + content+ "<!-- end of file : "+ inputPath 
                    //+" -->"
                    Tag = "link",
                    Content = "<link rel=\"stylesheet\" href=\"" + fullPath +"\"/>"

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
        /// Parses a block containing a CSS include directive from the provided queue of lines.
        /// </summary>
        /// <param name="lines">A queue containing the remaining lines of the document.</param>
        /// <returns>
        /// A collection containing a single <see cref="MarkupElement"/> for the CSS link,
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
        /// Gets the number of CSS include elements parsed.
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
