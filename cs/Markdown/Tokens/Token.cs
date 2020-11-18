using System.Collections.Generic;

namespace Markdown
{
    public abstract class Token
    {
        public int StartPosition;
        public int Length;
        public Token Parent;

        public Token(int startPosition = 0, int length = 0, Token parent = null)
        {
            StartPosition = startPosition;
            Length = length;
            Parent = parent;
        }

        public IEnumerable<Token> EnumerateParents()
        {
            for (var currentParent = Parent; currentParent != null; currentParent = currentParent.Parent)
                yield return currentParent;
        }
    }
}