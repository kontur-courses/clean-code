using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class Token
    {
        public static List<Token> SetRelations(List<Token> tokens)
        {
            var sortedTokens = tokens
                .Where(t => t.Length != 0)
                .OrderBy(t => t.Length)
                .ThenBy(t => t.Begin)
                .ToList();
            var childs = new List<Token>();
            for (int i = 0; i < sortedTokens.Count; i++)
                for (int j = i + 1; j < sortedTokens.Count; j++)
                {
                    var parent = sortedTokens[j];
                    var child = sortedTokens[i];
                    if (parent is TagToken && parent.Begin <= child.Begin
                        && parent.End >= child.End)
                    {
                        childs.Add(sortedTokens[i]);
                        if (parent.AllowInners && !child.HasParent)
                        {
                            child.SetCoordinatesRelatively(parent);
                            parent._inners.Add(child);
                        }
                    }
                }
            return sortedTokens

                .Except(childs)
                .OrderBy(t => t.Begin)
                .ToList();

            //if (!child.HasParent)
            //{
            //    child.SetCoordinatesRelatively(parent);
            //    parent._inners.Add(child);
            //}
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
