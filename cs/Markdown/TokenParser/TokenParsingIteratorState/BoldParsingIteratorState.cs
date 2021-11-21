﻿using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class BoldParsingIteratorState : UnderscoreParsingIteratorState
    {
        public BoldParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TokenNode Parse() => ParseUnderscore(Token.Bold);
    }
}