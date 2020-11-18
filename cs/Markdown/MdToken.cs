using System.Collections.Generic;

namespace Markdown
{
    public abstract class MdToken
    {
        public int StartPosition;
        public int Length;
        public MdToken Parent;

        public MdToken(int startPosition = 0, int length = 0, MdToken parent = null)
        {
            StartPosition = startPosition;
            Length = length;
            Parent = parent;
        }

        public IEnumerable<MdToken> EnumerateParents()
        {
            for (var currentParent = Parent; currentParent != null; currentParent = currentParent.Parent)
                yield return currentParent;
        }
    }
}