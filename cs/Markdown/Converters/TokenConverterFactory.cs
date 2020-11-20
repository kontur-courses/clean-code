using System;
using System.Collections.Generic;

namespace Markdown.Converters
{
    public class TokenConverterFactory : ITokenConverterFactory
    {
        private readonly Dictionary<TokenType, ITokenConverter> tokenConverters;

        public TokenConverterFactory()
        {
            tokenConverters = new Dictionary<TokenType, ITokenConverter>();
        }

        public ITokenConverter GetTokenConverter(TokenType tokenType, IConverter converter)
        {
            if (tokenConverters.TryGetValue(tokenType, out var tokenConverter))
                return tokenConverter;

            tokenConverter = tokenType switch
            {
                TokenType.Emphasized => new EmphasizedTagTokenConverter(converter),
                TokenType.Strong => new StrongTagTokenConverter(converter),
                TokenType.Header => new HeaderTagTokenConverter(converter),
                TokenType.Text => new PlainTextTokenConverter(),
                _ => throw new ArgumentOutOfRangeException($"Unknown token type {tokenType}")
            };

            tokenConverters[tokenType] = tokenConverter;

            return tokenConverter;
        }
    }
}