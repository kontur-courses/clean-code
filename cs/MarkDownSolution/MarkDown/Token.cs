using System.Collections.Generic;

namespace MarkDown
{
    public class Token
    {
        public bool closed { get; private set; }

        public int start { get; }

        public int length { get; private set; }


        public readonly List<Token> nestedTokens = new();

        public Token fatherToken;

        public Token(int start, int length)
        {
            this.start = start;
            this.length = length;
            closed = true;
        }

        public Token(int start)
        {
            this.start = start;
            closed = false;
        }

        public void SetLength(int length)
        {
            this.length = length;
            closed = true;
        }

        public void AddNestedToken(Token token)
        {
            nestedTokens.Add(token);
            token.fatherToken = this;
        }

        public void RemoveNestedToken(Token token)
        {
            nestedTokens.Remove(token);
        }

        public List<Token> GetNestedTokens()
        {
            return nestedTokens;
        }
    }
}
