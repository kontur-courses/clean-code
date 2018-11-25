using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokens = new TokenFinder().FindTokens(paragraph);

            //var validTokens = tokens;
            var validHtmlTags = new TokenValidator().SeparateByParagraphs(tokens, paragraph);
            //var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, validHtmlTags);
            new TokenValidator().FillParagraphWithHtmlTags(validHtmlTags);
            return "";
        }
    }
}
