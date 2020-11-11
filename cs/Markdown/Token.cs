using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        private List<Token> nestedTokens;

        public int Position { get; set; }
        public int Lenght { get; private set; }
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public IReadOnlyList<Token> NestedTokens => nestedTokens;

        public Token(int position, string value, TokenType type)
        {
            Position = position;
            Value = value;
            Type = type;
            nestedTokens = new List<Token>();
        }

        public void SetNestedTokens(List<Token> token)
        {
            nestedTokens = token;
        }
    }
}
