using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Token
    {
        public static void SetRelation(Token parent, Token child)
        {
            if (!child.HasParent)
            {
                child.SetCoordinatesRelatively(parent);
                parent._inners.Add(child);
            }
        }

        public virtual int RenderDelta => 0;
        public bool HasParent { get; private set; }
        public int Begin { get; private set; }
        public int End { get; private set; }
        public int Length => End - Begin;
        public virtual bool AllowInners => false;

        protected List<Token> _inners;
        public List<Token> Inners => _inners.ToList();

        public Token(int begin, int end)
        {
            Begin = begin;
            End = end;
            _inners = new List<Token>();
            HasParent = false;
        }

        public virtual string Render(string str, int offset = 0)
        {
            return str.Substring(Begin + offset, Length);
        }

        private void SetCoordinatesRelatively(Token parent)
        {
            HasParent = true;
            Begin -= parent.Begin;
            End -= parent.Begin;
        }
    }
}
