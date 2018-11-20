using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokens = new InlineTokenFinder().FindInlineTokensInMdText(paragraph);

            var inlineTokens = new InlineTokensValidator().GetPositionsForTokens(tokens);
            var startingTokens = new StartingTokenFinder().FindStartingTokens(paragraph);

            var validTokens = inlineTokens.Concat(startingTokens);
            //передавать поток токенов 
            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, tokens);

            return htmlParagraph;
        }
    }
}
