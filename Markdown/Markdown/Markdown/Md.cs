using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Md
    { 

        public static string Render(string MarkdownString)
        {
            
            var tokens=TokenParser.GetTokens(MarkdownString);
            var htmlString = HtmlParser.Parse(tokens, MarkdownString);
            return htmlString;
        }
    }
}
