using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenConverters
{
    public class EmphasizedTokenConverter : ITokenConverter
    {
        public TokenType TokenType { get; }
        private string TagText { get; }

        public EmphasizedTokenConverter()
        {
            TokenType = TokenType.Emphasized;
            TagText = "em";
        }

        public string ToString(TextToken token, Dictionary<TokenType, ITokenConverter> tokenConverters)
        {
            
            var htmlText = new StringBuilder();
            htmlText.Append("<em>");
            htmlText.Append(new HTMLConverter(tokenConverters).GetHtml(token.SubTokens));

            htmlText.Append("</em>");
            return htmlText.ToString();
        }
    }
}