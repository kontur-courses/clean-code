using System.Collections.Generic;

namespace Markdown
{
    public class TokenConverterFactory : ITokenConverterFactory
    {
        private readonly Dictionary<TokenType, ITagTokenConverter> tokenConverters;

        public TokenConverterFactory()
        {
            tokenConverters = new Dictionary<TokenType, ITagTokenConverter>
            {
                {TokenType.Text, new TextTokenConverter()},
                {TokenType.Emphasized, new EmphasizeTokenConverter()},
                {TokenType.Header, new HeaderTokenConverter()},
                {TokenType.Strong, new StrongTokenConverter()}
            };
        }

        public ITagTokenConverter GetTokenConverter(TokenType tokenType)
        {
            return tokenConverters[tokenType];
        }
    }
}