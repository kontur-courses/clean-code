using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenConverters
{
    public class HeaderTokenConverter : ITokenConverter
    {
        private TokenType Type { get; }
        private string TagText { get; }

        public HeaderTokenConverter()
        {
            Type = TokenType.Header;
            TagText = "h1";
        }

        public string ConvertTokenToString(TextToken token, IReadOnlyCollection<ITokenConverter> tokenConverters)
        {
            if (token.Type != Type)
                return null;

            var htmlText = new StringBuilder();
            htmlText.Append("<" + TagText + ">");
            foreach (var subToken in token.SubTokens)
            {
                foreach (var tokenConverter in tokenConverters)
                {
                    var subTokenString = tokenConverter.ConvertTokenToString(subToken, tokenConverters);
                    if (subTokenString != null)
                    {
                        htmlText.Append(subTokenString);
                    }
                }
            }

            htmlText.Append("</" + TagText + ">");
            return htmlText.ToString();
        }
    }
}