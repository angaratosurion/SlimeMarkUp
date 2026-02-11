using SlimeMarkUp.Core;
using SlimeMarkUp.Core.Extensions;
using SlimeMarkUp.Core.Extensions.SlimeMarkup;
using SlimeMarkUp.Core.Models;
using SlimeMarkUp.Tools;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlimeMarkUp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new MarkupParser(new List<IBlockMarkupExtension>
            {
                new HeaderExtension(),
                new ImageExtension(),
                new TableExtension(),
                new ListExtension(),
                new CodeBlockExtension(),
                new BlockquoteExtension(),
                new InlineStyleExtension(),
                new LinkExtension(),
               //  new IncludeExtension(),  
                new IncludeCSSExtension()
                 , new IncludeScriptExtension(),
                new HorizontalRuleExtension() ,new EscapeCharsExtension(),
                new HtmlIgnoreExceptIncludeExtension()

            });

            string input = File.ReadAllText("input.txt");
            
            var props =  DocumentPropertiesLoader.Load(input);

             
            DocumentProperties? docProps = props;
            
            var elements = parser.Parse(input, "input.yaml");

            var renderer = new HtmlRenderer();
            string html = renderer.Render(elements);
            ////elements= parser.Parse(html);
            ////html = renderer.Render(elements);


            File.WriteAllText("output.html", html);
            if (docProps != null)
            {
                Console.WriteLine($"File Name: {docProps.Filename}");
                Console.WriteLine($"Author: {docProps.Author}");
                // κλπ...
            }
            else
            {
                Console.WriteLine("Δεν βρέθηκαν ιδιότητες εγγράφου.");
            }
            Console.WriteLine("HTML export complete. Check output.html");
            var settings = new ConverterSettings { AddExtraNewLines = true };
            HtmlToSlimeMarkUpConverter htmlToSlimeMarkUpConverter = new 
                HtmlToSlimeMarkUpConverter(settings);
            htmlToSlimeMarkUpConverter.ConvertToFile(html,"output.md");
            Console.WriteLine("Markup expoerted to output.md");
        }
    }
}
