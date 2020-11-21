using System.Collections.Generic;

namespace Markdown
{
    public abstract class TokenWithSubTokens : Token
    {
        protected TokenWithSubTokens(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }

        private readonly List<Token> subTokens = new List<Token>();

        public void AddSubtoken(Token subToken)
        {
            subTokens.Add(subToken);
            subToken.Parent = this;
            Length += subToken.Length;
        }

        public void RemoveSubtokenAt(int index)
        {
            Length -= subTokens[index].Length;
            subTokens.RemoveAt(index);
        }

        public void RemoveLastSubtoken() => RemoveSubtokenAt(subTokens.Count - 1);

        public IEnumerable<Token> EnumerateSubtokens() => subTokens;

        public int GetSubtokenCount() => subTokens.Count;
    }
}