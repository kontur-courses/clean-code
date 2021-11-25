using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class LexContext
    {
        public const char EndOfFile = '\0';
        private readonly string text;

        private int position;

        public LexContext(string text) => this.text = text;

        public char Current => Peek(0);

        public char Lookahead => Peek(1);

        public IEnumerable<Token> Lex()
        {
            do
            {
                yield return LexConfig.GetLexer(Current)(this);
                MoveToNextSymbol();
            } while (Current != EndOfFile);
        }

        public void MoveToNextSymbol() => position++;
        public void MoveToPreviousSymbol() => position--;

        private char Peek(int offset)
        {
            var index = position + offset;

            return index >= text.Length ? EndOfFile : text[index];
        }
    }
}