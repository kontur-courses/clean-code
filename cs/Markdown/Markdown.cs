using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        private static readonly List<TokenType> TokensTypes = new List<TokenType>
        {
            new TokenType("simpleUnderscore", "_", "em"),
            new TokenType("doubleUnderscore", "__", "strong")
        };

        public string Render(string paragraph)
        {
            var tokens = new TokenFinder().FindTokensInMdText(paragraph, TokensTypes);
            var tokensPositions = new TokensValidator().GetPositionsForTokens(tokens);
            var htmlParagraph = new Md2HtmlTranslator().TranslateMdToHtml(paragraph, tokensPositions);

            return htmlParagraph;
        }
    }
}
