using System;
using System.Collections.Generic;

namespace Markdown
{
    public class LexIterator
    {
        private static readonly char[] SpecialCharacters =
        {
            '_',
            '\\',
            '#',
            '\n'
        };

        private readonly string text;
        private int currentIndex;

        public LexIterator(string text)
        {
            this.text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public IEnumerable<Token> Lex()
        {
            if (text == string.Empty) yield return new Token(TokenType.Text, string.Empty);
            while (currentIndex < text.Length)
            {
                var symbol = text[currentIndex];
                yield return symbol switch
                {
                    '_' => LexUnderscore(),
                    '\\' => new Token(TokenType.Escape, "\\"),
                    '#' => new Token(TokenType.Header1, "#"),
                    '\n' => new Token(TokenType.NewLine, "\n"),
                    _ => LexText()
                };
                currentIndex++;
            }
        }

        private Token LexUnderscore()
        {
            if (currentIndex + 1 < text.Length && text[currentIndex + 1] == '_')
            {
                currentIndex++;
                return new Token(TokenType.Bold, "__");
            }

            return new Token(TokenType.Cursive, "_");
        }

        private Token LexText()
        {
            var start = currentIndex;
            var end = text.IndexOfAny(SpecialCharacters, start);
            if (end == -1) end = text.Length;
            currentIndex = end - 1;
            return new Token(TokenType.Text, text.Substring(start, end - start));
        }
    }
}