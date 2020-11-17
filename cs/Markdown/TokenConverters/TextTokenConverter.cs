using System.Collections.Generic;

namespace Markdown.TokenConverters
{
    public class TextTokenConverter : ITokenConverter
    {
        public TokenType TokenType { get; }

        public TextTokenConverter()
        {
            TokenType = TokenType.Text;
        }

        public string ToString(TextToken token, Dictionary<TokenType,ITokenConverter> tokenConverters)
        {
            return token.Text;
        }
    }
}