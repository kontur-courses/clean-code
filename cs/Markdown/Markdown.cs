using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var validTokens = new TokenFinder().FindTokens(paragraph);

            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, validTokens);

            return htmlParagraph;
        }
    }
}
