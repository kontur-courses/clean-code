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
            var validHtmlTags = new TokenValidator().ValidInlineTokens(tokens.Where(t =>
                t.TokenType.TokenLocationType == TokenLocationType.InlineToken));
            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, validHtmlTags);

            return htmlParagraph;
        }
    }
}
