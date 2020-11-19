using System;
using System.Collections.Generic;

namespace Markdown.Converters
{
    public class TokenConverterFactory : ITokenConverterFactory
    {
        private readonly Dictionary<TokenType, ITagTokenConverter> tokenConverters;

        public TokenConverterFactory()
        {
            tokenConverters = new Dictionary<TokenType, ITagTokenConverter>();
        }

        public ITagTokenConverter GetTokenConverter(TokenType tokenType, IConverter converter)
        {
            if (tokenConverters.TryGetValue(tokenType, out var tokenConverter))
                return tokenConverter;

            tokenConverter = tokenType switch
            {
                TokenType.Emphasized => new EmphasizedTokenConverter(converter),
                TokenType.Strong => new StrongTokenConverter(converter),
                TokenType.Header => new HeaderTokenConverter(converter),
                TokenType.Text => new PlainTokenConverter(),
                _ => throw new ArgumentOutOfRangeException($"Unknown token type {tokenType}")
            };

            tokenConverters[tokenType] = tokenConverter;

            return tokenConverter;
        }
    }
}