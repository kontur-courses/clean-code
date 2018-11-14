using System;
using System.Collections.Generic;
using Markdown.Markups;

namespace Markdown
{
    class TokenReader
    {
        private readonly string text;
        private readonly List<Markup> markups;
        private int currentPosition;

        public TokenReader(string text, List<Markup> markups)
        {
            this.text = text;
            this.markups = markups;
        }

        public bool StillHasTokens()
        {
            return currentPosition < text.Length;
        }

        public Token NextToken()
        {
            var token = ReadTokenWithMarkup() ?? ReadPlainTextToken();
            return token;
        }

        private Token ReadTokenWithMarkup()
        {
            foreach (var markup in markups)
            {

            }
        }

        private Token ReadPlainTextToken()
        {

        }
    }
}
