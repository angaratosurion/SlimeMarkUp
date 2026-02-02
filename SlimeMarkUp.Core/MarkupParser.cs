using SlimeMarkUp.Core.Extensions.SlimeMarkup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq;

using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SlimeMarkUp.Core
{
    /// <summary>
    /// Provides functionality to parse custom markup text into a collection of structured markup elements, supporting
    /// extensible block-level parsing via registered markup extensions.
    /// </summary>
    /// <remarks>The MarkupParser supports extensibility by allowing custom implementations of
    /// IBlockMarkupExtension to be provided. These extensions are used to parse specific block-level constructs within
    /// the markup text. The parser also handles the removal of YAML front matter and document properties blocks, and
    /// includes support for processing include tags. Thread safety is not guaranteed; concurrent access to the same
    /// instance should be synchronized by the caller.</remarks>
    public class MarkupParser
    {
        private readonly List<IBlockMarkupExtension> _extensions;
        private static readonly Regex escaperegex =
          new Regex(@"^(?:\s{0,3})\d+[.)]\s+", RegexOptions.Compiled);
        IncludeTagHandler includeTagHandler = new IncludeTagHandler();
       /// <summary>
       /// Initializes a new instance of the MarkupParser class using the specified collection of block markup
       /// extensions.
       /// </summary>
       /// <remarks>The provided extensions determine how markup blocks are processed. The order in which
       /// extensions are applied may affect parsing results if multiple extensions handle similar markup.</remarks>
       /// <param name="extensions">A collection of IBlockMarkupExtension instances to be used by the parser. The extensions are applied in
       /// ascending order based on their Order property. Cannot be null.</param>
        public MarkupParser(IEnumerable<IBlockMarkupExtension> extensions)
        {
            _extensions = extensions.OrderBy(x => x.Order).ToList();
        }

    /// <summary>
    /// Parses the specified markup text and returns a collection of markup elements representing its structure.
    /// </summary>
    /// <remarks>YAML front matter and document properties blocks at the beginning of the input are
    /// automatically excluded from parsing. Lines not recognized by registered extensions are treated as paragraphs by
    /// default. The method supports extensible parsing via registered markup extensions.</remarks>
    /// <param name="text">The input text containing markup to be parsed. May include YAML front matter or document properties blocks,
    /// which will be ignored during parsing.</param>
    /// <returns>A list of <see cref="MarkupElement"/> objects representing the parsed elements of the markup text. The list will
    /// be empty if no elements are found.</returns>
        public List<MarkupElement> Parse(string text)
        {

          
            // Αφαίρεσε YAML από το input
            string markupOnly = Regex.Replace(text,
                @"^\s*---\s*\r?\n(.*?)\r?\n\s*---\s*", "",
                RegexOptions.Singleline);
            text = markupOnly;
            

            var elements = new List<MarkupElement>();
             text=PreParse(text);
            
            var lines = new Queue<string>(text.Split('\n').Select(l => l.TrimEnd()));
            

            while (lines.Count > 0)
            {
                var line = lines.Peek();

                if (string.IsNullOrWhiteSpace(line))
                {
                    lines.Dequeue();
                    continue;
                }

                // Αγνόησε ολόκληρο YAML block αν ξεκινάει από document_properties:
                if (line.StartsWith("document_properties:"))
                {
                    lines.Dequeue(); // Αφαίρεσε το document_properties:

                    // Αγνόησε όλες τις γραμμές με indent (κενά ή tab) ή μέχρι να τελειώσουν οι γραμμές
                    while (lines.Count > 0)
                    {
                        var nextLine = lines.Peek();

                        // Αν η γραμμή είναι κενή ή δεν έχει αρχικό indent, σταμάτα
                        if (string.IsNullOrWhiteSpace(nextLine) || !StartsWithIndent(nextLine))
                            break;

                        lines.Dequeue();
                    }
                    continue; // Επανέλαβε το loop για την επόμενη γραμμή
                }

                var matched = false;
             

                    foreach (var ext in _extensions)
                    {
 
                        if (ext.CanParse(line))
                        {
                            var blockElements = ext.ParseBlock(lines);
                            if (blockElements != null)
                            {
                                elements.AddRange(blockElements);
                                matched = true;
                                break;
                            }
                        }
                    
                }

                if (!matched  )
                {
                    // Default fallback: treat as paragraph
                   
                    var paragraph = lines.Dequeue();
                     elements.Add(new MarkupElement { Tag = "p", Content = paragraph });
                }
            }

            return elements;
        }
        /// <summary>
        /// Parses the specified markup text and returns a list of markup elements. Optionally saves extracted document
        /// properties to a file if a property file path is provided.
        /// </summary>
        /// <remarks>If <paramref name="propfile"/> is not <see langword="null"/>, any document properties
        /// found in the markup text are extracted and saved to the specified file before parsing the markup elements.
        /// The method does not validate the existence or accessibility of the file path.</remarks>
        /// <param name="text">The markup text to parse. This text may contain embedded document properties in YAML format.</param>
        /// <param name="propfile">The file path to which extracted document properties will be saved. If <paramref name="propfile"/> is <see
        /// langword="null"/>, document properties are not saved.</param>
        /// <returns>A list of <see cref="MarkupElement"/> objects representing the parsed elements from the markup text.</returns>
        public List<MarkupElement> Parse(string text , string propfile)
        {


            // Αφαίρεσε YAML από το input
            if (propfile!=null)
            {
                var prps = DocumentPropertiesLoader.Load(text);
                if ( prps!=null)
                {
                    DocumentPropertiesLoader.SaveToFile(propfile, prps);
                }
            }


           List<MarkupElement> elements = Parse(text);
            

            return elements;
        }
        /// <summary>
        /// Determines whether the specified line begins with a whitespace character, such as a space or tab.
        /// </summary>
        /// <param name="line">The line of text to evaluate. Can be null or empty.</param>
        /// <returns>true if the first character of the line is a whitespace character; otherwise, false.</returns>

        private bool StartsWithIndent(string line)
        {
            if (string.IsNullOrEmpty(line))
                return false;

            // Επιστρέφει true αν το πρώτο char είναι κενό ή tab
            return char.IsWhiteSpace(line[0]);
        }
        /// <summary>
        /// Processes the specified text, handling include tags and formatting lines for further parsing or rendering.
        /// </summary>
        /// <remarks>Lines containing include tags are parsed and expanded using the include tag handler.
        /// Lines without include tags are preserved with formatting. Empty or whitespace-only lines are
        /// ignored.</remarks>
        /// <param name="text">The input text to be pre-parsed. Each line may contain include tags or plain content. Cannot be null.</param>
        /// <returns>A string containing the processed text with include tags expanded and lines formatted. The returned string
        /// may include additional content based on include tag handling.</returns>
       string  PreParse(string text)
        {
            string ap=" ";
            var lines = new Queue<string>(text.Split('\n').Select(l => l.TrimEnd()));
            
           
          foreach( var line in lines)
            {
               // var line = lines.Peek();

                if (string.IsNullOrWhiteSpace(line))
                {
                    //lines.Dequeue();
                    continue;
                }
                
                  
                     if (includeTagHandler.CanParse(line)  )
                    {
                        var blockElements = includeTagHandler.Parse(line);
                        if (blockElements != null)
                        {
                           ap+= blockElements;
                        //break;
                        }
                    }
                     else
                {
                    ap += line+"\n";
                }

                
            }
            //var html = new HtmlRenderer();
           // ap= html.Render(elements.ToArray());
            return ap;
        }
    }
    
}
