using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokens = new InlineTokenFinder().FindInlineTokensInMdText(paragraph);
            var inlineTokensPositions = new InlineTokensValidator().GetPositionsForTokens(tokens);
            var startingTokens = new StartingTokenFinder().FindStartingTokens(paragraph);

            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, inlineTokensPositions, startingTokens);

            return htmlParagraph;
        }
    }
}
