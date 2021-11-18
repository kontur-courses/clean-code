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
        private readonly char[] specSymbols = {EndOfFile, '_', '/', '\n', '#'};
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
                    '/' => new Token(TokenType.Escape, "/"),
                    '\n' => new Token(TokenType.NewLine, "\n"),
                    '#' => new Token(TokenType.Header1, "#"),
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
                return new Token(TokenType.Italics, "_");
            position++;
            return new Token(TokenType.Bold, "__");
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
            return new Token(TokenType.Text, buffer.ToString());
        }
    }
}