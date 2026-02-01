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
    public class MarkupParser
    {
        private readonly List<IBlockMarkupExtension> _extensions;
        private static readonly Regex escaperegex =
          new Regex(@"^(?:\s{0,3})\d+[.)]\s+", RegexOptions.Compiled);
        IncludeTagHandler includeTagHandler = new IncludeTagHandler();
        public MarkupParser(IEnumerable<IBlockMarkupExtension> extensions)
        {
            _extensions = extensions.OrderBy(x => x.Order).ToList();
        }


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

        private bool StartsWithIndent(string line)
        {
            if (string.IsNullOrEmpty(line))
                return false;

            // Επιστρέφει true αν το πρώτο char είναι κενό ή tab
            return char.IsWhiteSpace(line[0]);
        }

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
