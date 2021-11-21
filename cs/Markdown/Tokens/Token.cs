using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tokens
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

        public abstract int RenderDelta { get; }
        public abstract bool HasParent { get; set; }
        public int Begin { get; private set; }
        public int End { get; private set; }
        public int Length => End - Begin;
        public abstract bool AllowInners { get; }

        protected List<Token> _inners;
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
