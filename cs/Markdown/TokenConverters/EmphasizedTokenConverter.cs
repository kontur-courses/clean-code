using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenConverters
{
    public class EmphasizedTokenConverter : ITokenConverter
    {
        private TokenType Type { get; }
        private string TagText { get; }

        public EmphasizedTokenConverter()
        {
            Type = TokenType.Emphasized;
            TagText = "em";
        }

        public string ConvertTokenToString(TextToken token, IReadOnlyCollection<ITokenConverter> tokenConverters)
        {
            if (token.Type != Type)
                return null;

            var htmlText = new StringBuilder();
            htmlText.Append("<em>");
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

            htmlText.Append("</em>");
            return htmlText.ToString();
        }
    }
}