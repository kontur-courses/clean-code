using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenConverters
{
    public class StrongTokenConverter : ITokenConverter
    {
        public TokenType TokenType { get; }
        private string TagText { get; }

        public StrongTokenConverter()
        {
            TokenType = TokenType.Strong;
            TagText = "strong";
        }

        public string ToString(TextToken token, Dictionary<TokenType, ITokenConverter> tokenConverters)
        {
            var htmlText = new StringBuilder();
            htmlText.Append("<strong>");
            htmlText.Append(new HTMLConverter(tokenConverters).GetHtml(token.SubTokens));

            htmlText.Append("</strong>");
            return htmlText.ToString();
        }
    }
}