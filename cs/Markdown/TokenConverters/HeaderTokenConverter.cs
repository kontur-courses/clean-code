using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenConverters
{
    public class HeaderTokenConverter : ITokenConverter
    {
        public TokenType TokenType { get; }
        private string TagText { get; }

        public HeaderTokenConverter()
        {
            TokenType = TokenType.Header;
            TagText = "h1";
        }

        public string ToString(TextToken token, Dictionary<TokenType, ITokenConverter> tokenConverters)
        {
            var htmlText = new StringBuilder();
            htmlText.Append("<" + TagText + ">");
            htmlText.Append(new HTMLConverter(tokenConverters).GetHtml(token.SubTokens));

            htmlText.Append("</" + TagText + ">");
            return htmlText.ToString();
        }
    }
}