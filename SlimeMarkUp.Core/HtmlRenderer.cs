using SlimeMarkUp.Core;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace SlimeMarkUp.Core
{
    /// <summary>
    /// Provides functionality to render a collection of markup elements into an HTML string.
    /// </summary>
    /// <remarks>Use this class to convert a sequence of markup elements, such as images, tables, and
    /// headings, into their corresponding HTML representation. The renderer handles specific tags with custom logic and
    /// wraps other elements in their respective HTML tags. This class is not thread-safe; create a separate instance
    /// for concurrent rendering operations.</remarks>
    public class HtmlRenderer
    {
        public string Render(IEnumerable<MarkupElement> elements)
        {
            var sb = new StringBuilder();
            foreach (var el in elements)
            {
                if (el.Tag == "img")
                {
                    sb.Append(el.Content); // περιέχει το <img ... /> ήδη
                }
                else if (el.Tag == "table")
                {
                    sb.Append(el.Content); // περιέχει όλο το table html
                }
                else if(el.Tag == "link")
                {
                    sb.Append(el.Content);
                }
                else if (el.Tag == "iframe")
                {
                    sb.Append(el.Content);
                }
                else if (el.Tag == "raw")
                {
                    sb.Append(el.Content);
                }
                else if ( el.Tag=="h0")
                {
                    var content = el.Content.Substring(1).Trim();
                    sb.Append($"<h1>{content}</h1>");
                }

                else
                {
                    if (el.Tag != "link")
                    {
                        sb.Append($"<{el.Tag}>{el.Content}</{el.Tag}>");
                    }
                    
                }
            }
            return sb.ToString();
        }
    }
}
