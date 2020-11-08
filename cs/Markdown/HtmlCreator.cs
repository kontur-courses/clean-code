using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class HtmlCreator
    {
        public static readonly Dictionary<Styles, string> StylesWithHtmlTags = new Dictionary<Styles, string>
        {
            {Styles.Italic, "em"}, {Styles.Bold, "strong"}, {Styles.Title, "h1"}
        };
        public static string AddHtmlTagToText(string text, Token token, Styles style)
        {
            throw new NotImplementedException();
        }
    }
}
