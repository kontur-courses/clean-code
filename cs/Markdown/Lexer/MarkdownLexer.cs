using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class MarkdownLexer : ILexer
    {
        private const char EndOfFile = '\0';

        private readonly Dictionary<char, Func<Token>> handlers;

        private int position;
        private string text;

        public MarkdownLexer()
        {
            handlers = new Dictionary<char, Func<Token>>
            {
                {'_', LexUnderscore},
                {'\\', () => Token.Escape},
                {'\n', () => Token.NewLine},
                {'#', LexHeader1}
            };
        }

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        public IEnumerable<Token> Lex(string inputText)
        {
            text = inputText ?? throw new ArgumentNullException(nameof(inputText));
            do
            {
                yield return handlers.GetValueOrDefault(Current, LexText)();
                position++;
            } while (Current != EndOfFile);
        }

        private char Peek(int offset)
        {
            var index = position + offset;

            return index >= text.Length ? EndOfFile : text[index];
        }

        private Token LexUnderscore()
        {
            if (Lookahead != '_')
                return Token.Italics;
            position++;
            return Token.Bold;
        }

        private Token LexHeader1()
        {
            if (Lookahead != ' ')
                return Token.Text("#");
            position++;
            return Token.Header1;
        }

        private Token LexText()
        {
            var buffer = new StringBuilder();
            while (!handlers.ContainsKey(Current) && Current != EndOfFile)
            {
                buffer.Append(Current);
                position++;
            }

            position--;
            return Token.Text(buffer.ToString());
        }
    }
}