using System.Collections.Generic;
using Markdown.Markups;

namespace Markdown
{
    public class Md
    {
        private readonly List<Markup> allMarkups = new List<Markup>()
        {
            new Underscore(),
            new DoubleUnderscore()
        };

        public string Render(string text)
        {
            return new MdParser().GetHtml(text, allMarkups);
        }
    }
}
