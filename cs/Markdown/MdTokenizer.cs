using System;
using System.Collections.Generic;

namespace Markdown.Tests
{
    class MdTokenizer
    {
        private readonly Dictionary<string, TokenType> stringTokenTypesDictionary = new Dictionary<string, TokenType>()
        {
            {"_", TokenType.Italic},
            {"__", TokenType.Bold},
        };
        private readonly MdTokenFixer fixer = new MdTokenFixer();

        // Текст разбивается на токены, учитывая escape-символы
        public List<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            var length = text.Length;
            var position = 0;
            while (position < length)
            {
                var token = GetNextToken(text, position);
                position += token.Length;
            }
            return fixer.FixTokens(tokens);
        }

        private Token GetNextToken(string text, int position)
        {
            throw new NotImplementedException();
        }
    }
}