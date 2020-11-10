using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Markdown
    {
        public static void Main()
        {
            var text = "";

            var tagsFounder = new TextHandler();
            var textTokens = tagsFounder.GetTextTokens(text);

            var htmlConverter = new HTMLConverter();
            var htmlString = htmlConverter.GetHTMLString(textTokens);

        }
    }
}
