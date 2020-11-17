using System.Collections.Generic;

namespace Markdown
{
    public abstract class MdTokenWithSubTokens : MdToken
    {
        protected MdTokenWithSubTokens(int startPosition, int length = 0, MdToken parent = null) : base(startPosition, length, parent)
        {
        }

        public List<MdToken> SubTokens = new List<MdToken>();

        public void AddSubtoken(MdToken subToken)
        {
            SubTokens.Add(subToken);
            subToken.Parent = this;
            Length += subToken.Length;
        }

        public void RemoveSubtokenAt(int index)
        {
            Length -= SubTokens[index].Length;
            SubTokens.RemoveAt(index);
        }

        public void RemoveLastSubtoken() => RemoveSubtokenAt(SubTokens.Count - 1);

        public void SetSubtokenCount(int count)
        {
            while (SubTokens.Count > count) RemoveLastSubtoken();
        }
    }
}