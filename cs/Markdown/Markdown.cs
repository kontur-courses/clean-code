using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokens = new TokenFinder().GetTokensOpeningAndClosingPositions(paragraph);
            var validTokens = new TokensValidator().GetPositionsForTokens(tokens);
            return new Md2HtmlTranslator().TranslateMdToHtml(paragraph, validTokens);
        }
    }
}
