using System.Collections.Generic;
using System.Text;
using SlimeMarkUp.Core;

namespace SlimeMarkUp.Core
{
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
                if (el.Tag == "link")
                {
                    sb.Append(el.Content);
                }
                 
                else
                {
                    sb.Append($"<{el.Tag}>{el.Content}</{el.Tag}>");
                }
            }
            return sb.ToString();
        }
    }
}
