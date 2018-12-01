using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Markdown
    {
        private readonly List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation
                {Symbol = "__", Tag = "strong", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, EndIsNewLine = false},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "#", Tag = "h1", IsPaired = true, EndIsNewLine = true}
        };

        public string Render(string mdText)
        {
            var parser = new TokenParser(baseTokens);
            var tokensMap = parser.GetTokens(mdText);
            var compositeRule = CreateCompositor();
            var correctTokens = compositeRule.Apply(tokensMap);
            var tokenDictionary = GetDictionary(correctTokens);
            var htmlText = new StringBuilder();
            var allPositions = new HashSet<int>(correctTokens.Select(x => x.Position));
            for (var i = 0; i < mdText.Length; i++)
            {
                if (allPositions.Contains(i))
                {
                    var token = tokenDictionary[i];
                    htmlText.Append(GetTag(token));
                    if (token.Data.EndIsNewLine && token.TokenType == TokenType.End)
                    {
                        if (i < mdText.Length - 1)
                            htmlText.Append(Environment.NewLine);
                        i++;
                    }
                    else
                    {
                        i += token.Data.Symbol.Length - 1;
                    }
                }
                else
                {
                    htmlText.Append(mdText[i]);
                }
            }

            return htmlText.ToString();
        }

        private Dictionary<int, Token> GetDictionary(List<Token> allTokens)
        {
            var dictionary = new Dictionary<int, Token>();
            foreach (var token in allTokens) dictionary[token.Position] = token;

            return dictionary;
        }

        private CompositeRule CreateCompositor()
        {
            var firstRule = new UnpairedSymbolsRule(baseTokens);
            var secondRule = new DoubleUnderscoreBetweenUnderscoreRule();
            var thirdRule = new EscapedSymbolsBetweenGraveAccent();
            return new CompositeRule(new IRule[] {firstRule, secondRule, thirdRule});
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