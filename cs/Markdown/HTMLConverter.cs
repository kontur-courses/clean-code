using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HTMLConverter : IConverter
    {
        private readonly Dictionary<TokenType, string> tokenConverters;

        public HTMLConverter(Dictionary<TokenType, string> tokensText)
        {
            tokenConverters = tokensText;
        }

        public string ConvertTokens(IReadOnlyCollection<TextToken> textTokens)
        {
            var html = new StringBuilder();
            foreach (var token in textTokens)
            {
                var currentText = ToString(token);
                html.Append(currentText);
            }

            return html.ToString();
        }

        private string ToString(TextToken token)
        {
            if (token.Type == TokenType.Text)
                return token.Text;
            var htmlText = new StringBuilder();
            htmlText.Append($"<{tokenConverters[token.Type]}>");
            htmlText.Append(ConvertTokens(token.SubTokens));

            htmlText.Append($"</{tokenConverters[token.Type]}>");
            return htmlText.ToString();
        }
    }
}