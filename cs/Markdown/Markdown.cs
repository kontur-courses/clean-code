using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var inlineTokens = new InlineTokenFinder().FindInlineTokensInMdText(paragraph);
            var validInlineTokens = new InlineTokensValidator().GetValidTokens(inlineTokens);
            var startingTokens = new StartingTokenFinder().FindStartingTokens(paragraph);

            var validTokens = validInlineTokens.Union(startingTokens);

            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, validTokens);

            return htmlParagraph;
        }
    }
}
