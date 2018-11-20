using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokens = new InlineTokenFinder().FindInlineTokensInMdText(paragraph);
            var tokensPositions = new InlineTokensValidator().GetPositionsForTokens(tokens);
            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, tokensPositions);

            return htmlParagraph;
        }
    }
}
