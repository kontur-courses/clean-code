using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class LexIterator
    {
        private static readonly HashSet<char> SpecialCharacters = new()
        {
            '_',
            '\\',
            '\n'
        };

        private readonly string text;
        private int currentIndex;
        private bool isNewLine = true;

        public LexIterator(string text)
        {
            this.text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public IEnumerable<Token> Lex()
        {
            if (text == string.Empty) yield return Token.Text(string.Empty);
            while (currentIndex < text.Length)
            {
                var symbol = text[currentIndex];
                yield return symbol switch
                {
                    '_' => LexUnderscore(),
                    '\\' => LexEscape(),
                    '#' => LexHeader(),
                    '\n' => LexNewLine(),
                    _ => LexText()
                };
                currentIndex++;
            }
        }

        private Token LexUnderscore()
        {
            isNewLine = false;
            if (IsNextCharacter('_') )
            {
                currentIndex++;
                return Token.Bold;
            }

            return Token.Cursive;
        }

        private Token LexEscape()
        {
            isNewLine = false;
            return Token.Escape;
        }

        private Token LexHeader()
        {
            if (isNewLine && IsNextCharacter(' '))
            {
                currentIndex++;
                return Token.Header1;
            }

            return LexText();
        }

        private Token LexNewLine()
        {
            isNewLine = true;
            return Token.NewLine;
        }

        private Token LexText()
        {
            var start = currentIndex;
            var end = start + 1;
            for (; end < text.Length; end++)
            {
                if (isNewLine && text[end] == '#' || SpecialCharacters.Contains(text[end])) break;

                isNewLine = isNewLine && char.IsWhiteSpace(text[end]);
            }

            currentIndex = end - 1;
            return Token.Text(text.Substring(start, end - start));
        }

        private bool IsNextCharacter(char ch) => currentIndex + 1 < text.Length && text[currentIndex + 1] == ch;
    }
}