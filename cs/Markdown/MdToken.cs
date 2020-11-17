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
}