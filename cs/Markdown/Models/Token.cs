using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Models
{
    internal class Token
    {
        public int StartPosition { get; }
        public int Length { get; }
        public string Value { get; }
        public List<Token> InnerTokens { get; }

        public Token(string value, int startPosition)
        {
            Value = value;
            StartPosition = startPosition;
            Length = value.Length;
            InnerTokens = new List<Token>();
        }
    }
}
