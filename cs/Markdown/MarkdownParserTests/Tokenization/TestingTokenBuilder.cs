using System;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParserTests.Tokenization
{
    public class TestingTokenBuilder : ITokenBuilder
    {
        public string TokenSymbol { get; set; }

        public Token Create(TokenizationContext context)
        {
            Creating?.Invoke(context);
            return TokenToReturn;
        }

        public bool CanCreateOnPosition(TokenPosition position)
        {
            Validating?.Invoke(position);
            return CanCreate;
        }

        public event Action<TokenizationContext> Creating;
        public event Action<TokenPosition> Validating;

        public bool CanCreate { get; set; } = false;
        public Token TokenToReturn { get; set; }
    }
}