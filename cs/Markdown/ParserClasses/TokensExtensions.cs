using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TokenizerClasses;

namespace Markdown.ParserClasses
{
    public static class TokensExtensions
    {
        public static Token PeekFirst(this List<Token> tokens)
        {
            if (tokens.Count == 0)
                throw new InvalidOperationException();

            return tokens.First();
        }

        public static Token PopFirst(this List<Token> tokens)
        {
            if (tokens.Count == 0)
                throw new InvalidOperationException();

            var token = tokens.First();
            tokens.RemoveAt(0);

            return token;
        }

        public static void RemoveLast(this List<Token> tokens)
        {
            if (tokens.Count > 0)
                tokens.RemoveAt(tokens.Count - 1);
        }

        public static Token Penultimate(this List<Token> tokens)
        {
            if (tokens.Count < 2)
                throw new InvalidOperationException();

            return tokens[tokens.Count - 2];
        }
    }
}