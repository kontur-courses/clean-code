using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        public string ConvertTokensToHtml(List<Token> tokens)
        {
            var sortedTokens = tokens.OrderBy(x => x.Position);
            var result = new StringBuilder();

            foreach (var token in sortedTokens)
            {
                switch (token.Type)
                {
                    case TokenType.Heading:
                        result.Append($"<h1>{token.ValueWithoutTags()}</h1>");
                        break;
                    case TokenType.Strong:
                        result.Append($"<strong>{token.ValueWithoutTags()}</strong>");
                        break;
                    case TokenType.Emphasized:
                        result.Append($"<em>{token.ValueWithoutTags()}</em>");
                        break;
                    case TokenType.PlainText:
                        result.Append(token.ValueWithoutTags());
                        break;
                }
            }

            return result.ToString();
        }
    }
}