using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class Token
    {
        public static void SetRelation(Token parent, Token child)
        {
            if (!child.HasParent)
            {
                child.SetCoordinatesRelatively(parent);
                parent._inners.Add(child);
            }
        }

        protected List<Token> _inners;
        public abstract int RenderDelta { get; }
        public bool HasParent { get; protected set; }
        public int Begin { get; protected set; }
        public int End { get; protected set; }
        public int Length => End - Begin;
        public abstract bool AllowInners { get; }
        public List<Token> Inners => _inners.ToList();

        public Token(int begin, int end)
        {
            Begin = begin;
            End = end;
            _inners = new List<Token>();
            HasParent = false;
        }

        public abstract string Render(string str);

        private void SetCoordinatesRelatively(Token parent)
        {
            HasParent = true;
            Begin -= parent.Begin;
            End -= parent.Begin;
        }
    }
}
