using System.Collections.Generic;

namespace MarkDown
{
    public class Token
    {
        public virtual string OpenedHtmlTag => "";

        public virtual string ClosedHtmlTag => "";

        public virtual int RawLengthOpen => 0;

        public virtual int RawLengthClose => 0;

        public bool Closed { get; private set; }

        public int Start { get; }

        public int Length { get; private set; }


        public readonly List<Token> nestedTokens = new();

        public Token fatherToken;

        public Token(int start, int length)
        {
            this.Start = start;
            this.Length = length;
            Closed = true;
        }

        public Token(int start)
        {
            this.Start = start;
            Closed = false;
        }

        public void SetLength(int length)
        {
            this.Length = length;
            Closed = true;
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
