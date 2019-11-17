using System;
using System.Collections.Generic;

namespace Markdown
{
    class TokenReader
    {
        private readonly List<TokenDescription> tokenDescriptions;

        public TokenReader(List<TokenDescription> tokenDescriptions)
        {
            this.tokenDescriptions = tokenDescriptions;
        }

        public List<Token> SplitToTokens(string text)
        {
            var tokens = new List<Token>();
            var currentPosition = 0;
            while (currentPosition < text.Length)
            {
                var isTokenAdded = false;
                foreach (var tokenDescription in tokenDescriptions)
                {
                    var token = tokenDescription.ReadToken(text, currentPosition);
                    if (token.IsEmpty)
                        continue;

                    currentPosition += token.Length;
                    tokens.Add(token);
                    isTokenAdded = true;
                    break;
                }
                if (!isTokenAdded)
                    throw new Exception("Correct token description not found");
            }

            return tokens;
        }

        public static Token ReadSubstringToken(string text, int position, string subString, TokenType tokenType)
        {
            if (position + subString.Length > text.Length)
                return Token.EmptyToken;

            var length = 0;
            while (length < subString.Length && text[position + length] == subString[length])
                length++;

            return length < subString.Length ? Token.EmptyToken : new Token(position, text.Substring(position, length), tokenType);
        }

        public static Token ReadTokenWithRuleForSymbols(string text, int position, Func<char, bool> rule, TokenType tokenType)
        {
            var i = position;
            while (i < text.Length && rule(text[i]))
                i++;

            return new Token(position, text.Substring(position, i - position), tokenType);
        }

        public static Token ReadEscapedSymbol(string text, int position, char escapeSymbol)
        {
            if (text[position] != escapeSymbol || position >= text.Length - 1 || char.IsWhiteSpace(text[position + 1]))
                return Token.EmptyToken;

            return new Token(position, text.Substring(position, 2), TokenType.EscapedSymbol);
        }
    }
}
