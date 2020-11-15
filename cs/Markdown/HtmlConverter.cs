using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        public string ConvertTokens(List<Token> tokens)
        {
            var sortedTokens = tokens.OrderBy(x => x.Position);
            var result = new StringBuilder();
            var mapper = GetTokenTypeToStringMapper();

            foreach (var token in sortedTokens)
            {
                var tokenValue = token.GetValueWithoutTags();
                var tokenType = mapper[token.Type];

                result.Append(tokenType != "" ? $"<{tokenType}>{tokenValue}</{tokenType}>" : tokenValue);
            }

            return result.ToString();
        }

        private static Dictionary<TokenType, string> GetTokenTypeToStringMapper()
        {
            return new Dictionary<TokenType, string>() {
                {TokenType.Emphasized, "em"},
                {TokenType.Heading, "h1"},
                {TokenType.Strong, "strong"},
                {TokenType.PlainText, ""}
            };
        }
    }
}