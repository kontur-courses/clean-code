using System.Collections.Generic;
using System.Text;
using Markdown.Parser.Inline.Tokens;

namespace Markdown.Parser.Inline
{
    public static class Tokenizer
    {
        public static List<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            var buffer = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\\' && i + 1 < text.Length)
                {
                    buffer.Append(text[i + 1]);
                    i++;
                    continue;
                }

                if (c == '_')
                {
                    AddTokenText(tokens, buffer, i);

                    var currentToken = GetUnderscoreToken(text, i);
                    if (currentToken.Type == TokenType.Strong) i++;

                    tokens.Add(currentToken);
                    continue;
                }

                buffer.Append(c);
            }

            AddTokenText(tokens, buffer, text.Length);
            return tokens;
        }

        private static void AddTokenText(List<Token> tokens, StringBuilder buffer, int currentIndex)
        {
            if (buffer.Length == 0) return;

            var startPosition = currentIndex - buffer.Length;
            tokens.Add(new Token(TokenType.Text, buffer.ToString(), startPosition));
            buffer.Clear();
        }

        private static Token GetUnderscoreToken(string text, int startIndex)
        {
            var isStrong = startIndex + 1 < text.Length && text[startIndex + 1] == '_';

            var type = isStrong ? TokenType.Strong : TokenType.Em;
            var value = isStrong ? "__" : "_";
            var length = isStrong ? 2 : 1;

            var token = new Token(type, value, startIndex);
            SetTokenFlags(token, text, startIndex, length);

            return token;
        }

        private static void SetTokenFlags(Token token, string text, int startIndex, int length)
        {
            char? prev = startIndex > 0 ? text[startIndex - 1] : null;
            char? next = startIndex + length < text.Length ? text[startIndex + length] : null;

            if (next.HasValue && !char.IsWhiteSpace(next.Value))
                token.CanOpen = true;

            if (prev.HasValue && !char.IsWhiteSpace(prev.Value))
                token.CanClose = true;

            if (prev.HasValue && next.HasValue && char.IsDigit(prev.Value) && char.IsDigit(next.Value))
            {
                token.CanOpen = false;
                token.CanClose = false;
            }

            token.IsInsideWord = prev.HasValue && next.HasValue &&
                         char.IsLetter(prev.Value) &&
                         char.IsLetter(next.Value);
        }
    }
}