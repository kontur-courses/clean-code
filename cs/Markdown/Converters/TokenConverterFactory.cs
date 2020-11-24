using System;
using System.Collections.Generic;
using Markdown.Tokens;

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
                TokenType.Emphasized => new EmphasizedTokenConverter(converter),
                TokenType.Strong => new StrongTokenConverter(converter),
                TokenType.Heading => new HeadingTokenConverter(converter),
                TokenType.PlainText => new PlainTextTokenConverter(),
                TokenType.Image => new ImageTokenConverter(),
                _ => throw new ArgumentOutOfRangeException($"Unknown token type {tokenType}")
            };

            tokenConverters[tokenType] = tokenConverter;

            return tokenConverter;
        }
    }
}