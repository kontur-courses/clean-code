using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Markdown
    {
        public List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation
                {Symbol = "__", Tag = "strong", IsPaired = true, CountOfSpaces = 2, EndIsNewLine = false},
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "#", Tag = "h1", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = true}
        };

        public string Render(string mdText)
        {
            var parser = new TokenParser();
            var tokensMap = parser.GetTokens(mdText, baseTokens);
            var rules = CreateCompositor();
            var compositeRule = new CompositeRule(rules);

            var correctTokens = compositeRule.Apply(tokensMap, baseTokens);
            var htmlText = new StringBuilder();
            var allPositions = correctTokens.Select(x => x.Position).ToArray();
            for (var i = 0; i < mdText.Length; i++)
            {
                if (allPositions.Contains(i))
                {
                    var token = correctTokens.Find(x => x.Position == i);
                    htmlText.Append(GetTag(token));
                    i += token.Data.CountOfSpaces - 1;
                }
                else
                {
                    htmlText.Append(mdText[i]);
                }
            }

            return htmlText.ToString();
        }

        private IRule[] CreateCompositor()
        {
            var firstRule = new UnpairedSymbolsRule();
            var secondRule = new DoubleUnderscoreBetweenUnderscoreRule();
            return new IRule[] {firstRule, secondRule};
        }

        private string GetTag(Token token)
        {
            if (token.TokenType == TokenType.Escaped)
                return "";
            if (token.TokenType == TokenType.Ordinary)
                return token.Data.Symbol;
            return token.TokenType == TokenType.End ? $"</{token.Data.Tag}>" : $"<{token.Data.Tag}>";
        }
    }
}