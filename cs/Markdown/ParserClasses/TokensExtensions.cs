using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TokenizerClasses;

namespace Markdown.ParserClasses
{
    public static class TokensExtensions
    {
        public static Token PeekFirst(this Deque<Token> tokens)
        {
            if (tokens.Count == 0)
                throw new InvalidOperationException();

            return tokens.First();
        }

        public static Token PopFirst(this Deque<Token> tokens)
        {
            if (tokens.Count == 0)
                throw new InvalidOperationException();

            var token = tokens.First();
            tokens.RemoveAt(0);

            return token;
        }

        public static Token Penultimate(this Deque<Token> tokens)
        {
            if (tokens.Count < 2)
                throw new InvalidOperationException();

            return tokens.Get(tokens.Count - 2);
        }
    }
}