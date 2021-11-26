using System.Collections.Generic;

namespace MarkDown
{
    public class Token
    {
        public Token(int start, int length)
        {
            this.Start = start;
            this.Length = length;
            Closed = true;
            NestedTokens = new();
        }

        public Token(int start)
        {
            this.Start = start;
            Closed = false;
            NestedTokens = new();
        }

        public Token FatherToken { get; set; }
        public List<Token> NestedTokens { get; }
        public bool Closed { get; private set; }
        public int Start { get; }
        public int Length { get; private set; }
        public virtual bool CanBeOpened(string text, int i) => true;
        public virtual bool CanBeClosed(string text, int i) => true;
        public virtual bool IsFullLiner => false;
        public virtual string ClosedHtmlTag => string.Empty;
        public virtual int RawLengthOpen => 0;
        public virtual int RawLengthClose => 0;
        public virtual string OpenedHtmlTag => string.Empty;

        public void SetLength(int length)
        {
            this.Length = length;
            Closed = true;
        }

        public void AddNestedToken(Token token)
        {
            NestedTokens.Add(token);
            token.FatherToken = this;
        }

        public void RemoveNestedToken(Token token)
        {
            NestedTokens.Remove(token);
        }

        public List<Token> GetNestedTokens()
        {
            return NestedTokens;
        }

        public virtual Token CreateNewTokenOfSameType(int start)
        {
            return new Token(start);
        }
    }
}
