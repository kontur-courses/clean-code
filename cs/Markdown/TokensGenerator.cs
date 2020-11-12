using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TokensGenerator
    {
        private ITextParser TextParser { get; }

        public TokensGenerator(ITextParser textParser)
        {
            TextParser = textParser;
        }

        public List<Token> GetTokens(string text)
        {
            var tokens = TextParser.GetTokens(text);

            return tokens;
        }
    }
}