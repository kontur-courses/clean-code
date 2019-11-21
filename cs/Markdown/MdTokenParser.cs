using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tests
{
    class MdTokenParser
    {
        // Текст разбивается на токены, учитывая escape-символы
        public List<Token> Tokenize(string paragraph)
        {
            var tokens = new List<Token>();
            var length = paragraph.Length;
            var position = 0;
            while (position < length)
            {
                var token = GetNextToken(paragraph, position);
                position += token.Length;
                tokens.Add(token);
            }

            return tokens;
        }

        private Token GetNextToken(string paragraph, int position)
        {
            var currentSymbol = paragraph[position];
            return IsPartOfAnyNonTextToken(currentSymbol.ToString())
                ? GetNonTextToken(paragraph, position)
                : GetTextToken(paragraph, position);
        }

        private Token GetTextToken(string paragraph, int position)
        {
            var tokenLength = 0;
            var length = paragraph.Length;
            var currentTokenContent = "";
            var escaped = false;
            while (position < length)
            {
                var currentSymbol = paragraph[position];
                tokenLength++;
                position++;
                if (escaped)
                {
                    currentTokenContent += currentSymbol;
                    escaped = false;
                }
                else if (currentSymbol == '\\')
                {
                    escaped = true;
                }
                else
                {
                    currentTokenContent += currentSymbol;

                    var nextSymbol = TryGetNextSymbol(paragraph, position);
                    if (IsPartOfAnyNonTextToken(nextSymbol.ToString()))
                    {
                        return new Token(TokenType.Text, currentTokenContent, tokenLength);
                    }
                }
            }

            return new Token(TokenType.Text, currentTokenContent, tokenLength);
        }

        private Token GetNonTextToken(string paragraph, int position)
        {
            var length = paragraph.Length;
            var tokenLength = 0;
            var currentTokenContent = "";
            while (position < length)
            {
                var currentSymbol = paragraph[position];
                var nextContent = currentTokenContent + currentSymbol;
                if (IsPartOfAnyNonTextToken(nextContent))
                {
                    currentTokenContent = nextContent;
                    tokenLength++;
                    position++;
                }
                else
                {
                    break;
                }
            }

            return TokenTypesTranslator.GetTokenFromString(currentTokenContent);
        }

        private static char? TryGetNextSymbol(string paragraph, int position)
        {
            if (position < paragraph.Length)
                return paragraph[position];
            return null;
        }

        private static bool IsPartOfAnyNonTextToken(string substring)
        {
            return TokenTypesTranslator.GetSupportedTokens().Any(tokenTag => tokenTag.StartsWith(substring));
        }
    }
}