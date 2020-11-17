using System.Collections.Generic;

namespace Markdown
{
    public abstract class MdToken
    {
        public int StartPosition;
        public int Length;
        public MdToken ParentToken;

        public MdToken(int startPosition, int length = 0)
        {
            StartPosition = startPosition;
            Length = length;
        }

        public IEnumerable<MdToken> EnumerateParrents()
        {
            for (var currentParrent = ParentToken;currentParrent != null; currentParrent = currentParrent.ParentToken)
                yield return currentParrent;
        }
    }

    public abstract class MdTokenWithSubTokens : MdToken
    {
        protected MdTokenWithSubTokens(int startPosition, int length = 0) : base(startPosition, length) { }

        public List<MdToken> SubTokens = new List<MdToken>();

        public void AddSubtoken(MdToken subToken)
        {
            SubTokens.Add(subToken);
            subToken.ParentToken = this;
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