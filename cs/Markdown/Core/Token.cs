using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        private List<Token> nestedTokens;

        public int Position { get; }
        public readonly string value;
        public readonly TokenType type;
        public IReadOnlyList<Token> NestedTokens => nestedTokens;

        public Token(int position, string value, TokenType type)
        {
            Position = position;
            this.value = value;
            this.type = type;
            nestedTokens = new List<Token>();
        }

        public void SetNestedTokens(List<Token> token)
        {
            nestedTokens = token;
        }
    }
}
