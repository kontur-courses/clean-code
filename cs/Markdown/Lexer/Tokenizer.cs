using System;

namespace Markdown.Lexer
{
    internal class Tokenizer
    {
        private readonly string input;

        private int location;

        public bool End => location == input.Length;

        internal Tokenizer(string input)
        {
            this.input = input;
        }

        public Token GetNext()
        {
            throw new NotImplementedException();
        }

        public Token Peek()
        {
            throw new NotImplementedException();
        }
    }
}