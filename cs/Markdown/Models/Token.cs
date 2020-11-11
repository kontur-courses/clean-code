using System.Collections.Generic;

namespace Markdown.Models
{
    internal class Token
    {
        public int Length { get; }
        public string Value { get; }
        public List<Token> InnerTokens { get; }

        public Token(string value)
        {
            Value = value;
            Length = value.Length;
            InnerTokens = new List<Token>();
        }
    }
}
