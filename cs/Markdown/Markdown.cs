using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokensPositions = new TokenFinder().GetTokensOpeningAndClosingPositions(paragraph);
            var tokens =new TokensValidator().GetPositionsForTokens
                (tokensPositions.OpeningPositions, tokensPositions.ClosingPositions);
            return new Md2HtmlTranslator().TranslateMdToHtml(paragraph, tokens);
        }
    }
}
