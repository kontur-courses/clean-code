﻿using Markdown.Tokens;

namespace Markdown.Lexer.ConcreteLexers
{
    public class Header1Lexer : ConcreteLexer
    {
        public Header1Lexer(LexContext context) : base(context)
        {
        }

        public override Token Lex()
        {
            if (Context.Lookahead != ' ')
                return Token.Text("#");
            Context.NextSymbol();
            return Token.Header1;
        }
    }
}