using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class MarkdownLexer : ILexer
    {
        private const char EndOfFile = '\0';
        private readonly char[] specSymbols = {EndOfFile, '_', '\\', '\n', '#'};
        private int position;
        private string text;

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        public IEnumerable<Token> Lex(string inputText)
        {
            text = inputText ?? throw new ArgumentNullException(nameof(inputText));
            do
            {
                yield return Current switch
                {
                    '_' => ParseUnderscore(),
                    '\\' => Token.Escape,
                    '\n' => Token.NewLine,
                    '#' => ParseHeader1(),
                    _ => ParseText()
                };
                position++;
            } while (Current != EndOfFile);
        }

        private char Peek(int offset)
        {
            var index = position + offset;

            return index >= text.Length ? EndOfFile : text[index];
        }

        private Token ParseUnderscore()
        {
            if (Lookahead != '_')
                return Token.Italics;
            position++;
            return Token.Bold;
        }

        private Token ParseHeader1()
        {
            if (Lookahead != ' ')
                return Token.Text("#");
            position++;
            return Token.Header1;
        }

        private Token ParseText()
        {
            var buffer = new StringBuilder();
            while (!specSymbols.Contains(Current))
            {
                buffer.Append(Current);
                position++;
            }

            position--;
            return Token.Text(buffer.ToString());
        }
    }
}