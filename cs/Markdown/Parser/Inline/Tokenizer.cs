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
            var index = 0;

            while (index < text.Length)
            {
                var c = text[index];

                switch (c)
                {
                    case '\\':
                        index += HandleEscape(text, index, buffer);
                        break;

                    case '_':
                        index += HandleUnderscore(text, index, tokens, buffer);
                        break;

                    default:
                        buffer.Append(c);
                        index++;
                        break;
                }
            }

            AddTokenText(tokens, buffer, text.Length);
            return tokens;
        }

        private static int HandleEscape(string text, int i, StringBuilder buffer)
        {
            if (i + 2 < text.Length && text[i + 1] == '_' && text[i + 2] == '_')
            {
                buffer.Append("__");
                return 3;
            }

            if (i + 1 < text.Length)
            {
                buffer.Append(text[i + 1]);
                return 2;
            }

            buffer.Append('\\');
            return 1;
        }

        private static int HandleUnderscore(string text, int i, List<Token> tokens, StringBuilder buffer)
        {
            AddTokenText(tokens, buffer, i);

            var isStrong = i + 1 < text.Length && text[i + 1] == '_';
            var type = isStrong ? TokenType.Strong : TokenType.Em;
            var value = isStrong ? "__" : "_";
            var length = isStrong ? 2 : 1;

            var token = new Token(type, value, i);
            SetTokenFlags(token, text, i, length);
            tokens.Add(token);

            return length;
        }

        private static void AddTokenText(List<Token> tokens, StringBuilder buffer, int currentIndex)
        {
            if (buffer.Length == 0) return;

            var startPosition = currentIndex - buffer.Length;
            tokens.Add(new Token(TokenType.Text, buffer.ToString(), startPosition));
            buffer.Clear();
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