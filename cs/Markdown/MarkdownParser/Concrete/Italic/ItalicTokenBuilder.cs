using System;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Concrete.Italic
{
    public class ItalicTokenBuilder : TokenBuilder<ItalicToken>
    {
        public override string TokenSymbol { get; } = "_";
        public override ItalicToken Create(TokenizationContext context)
        {
            throw new NotImplementedException(); //TODO implement
        }

        public override bool CanCreateOnPosition(TokenPosition position)
        {
            throw new NotImplementedException(); //TODO implement
        }
    }
}