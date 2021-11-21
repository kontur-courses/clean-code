using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class StringToken
    {
        public static void SetRelation(StringToken parent, StringToken child)
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

        protected List<StringToken> _inners;
        public List<StringToken> Inners => _inners.ToList();

        public StringToken(int begin, int end)
        {
            Begin = begin;
            End = end;
            _inners = new List<StringToken>();
            HasParent = false;
        }

        public virtual string Render(string str)
        {
            return str.Substring(Begin, Length);
        }

        private void SetCoordinatesRelatively(StringToken parent)
        {
            HasParent = true;
            Begin -= parent.Begin;
            End -= parent.Begin;
        }
    }
}
