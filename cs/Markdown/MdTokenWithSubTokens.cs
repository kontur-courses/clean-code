using System.Collections.Generic;

namespace Markdown
{
    public abstract class MdTokenWithSubTokens : MdToken
    {
        protected MdTokenWithSubTokens(int startPosition, int length = 0, MdToken parent = null)
            : base(startPosition, length, parent)
        {
        }

        private readonly List<MdToken> subTokens = new List<MdToken>();

        public void AddSubtoken(MdToken subToken)
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

        public IEnumerable<MdToken> EnumerateSubtokens() => subTokens;

        public void SetSubtokenCount(int count)
        {
            while (subTokens.Count > count) RemoveLastSubtoken();
        }
    }
}