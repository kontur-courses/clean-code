﻿using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Italic
{
    public sealed class ItalicToken : PairedToken
    {
        public TokenPosition Position { get; }

        public ItalicToken(int startPosition, string rawValue, TokenPosition position)
            : base(startPosition, rawValue)
        {
            Position = position;
            Type = 0;
            if (!position.BeforeWhitespace())
                Type |= TokenType.Opening;
            if (!position.AfterWhitespace())
                Type |= TokenType.Closing;
        }

        public override TokenType Type { get; }
    }
}