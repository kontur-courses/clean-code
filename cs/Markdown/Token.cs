using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public Tag Tag;
        public int StartOpenMark;
        public int EndCloseMark;
        public int TextStart => StartOpenMark + Tag.OpenMark.Length - 1;
        public int TextEnd => EndCloseMark - Tag.CloseMark.Length;

        public Token(Tag tag, int startOpenMark = -1, int endCloseMark = -1)
        {
            Tag = tag;
            StartOpenMark = startOpenMark;
            EndCloseMark = endCloseMark;
        }

        public bool IntersectsWith(Token other)
        {
            return StartOpenMark > other.StartOpenMark && StartOpenMark < other.EndCloseMark && EndCloseMark > other.EndCloseMark
                   || other.StartOpenMark > StartOpenMark && other.StartOpenMark < EndCloseMark && other.EndCloseMark > EndCloseMark;
        }

        public Token FindParent(List<Token> tokens)
        {
            Token parent = null!;
            foreach (var t in tokens)
            {
                if (t.StartOpenMark >= StartOpenMark || t.EndCloseMark <= EndCloseMark) 
                    continue;
                if (parent == null! || t.StartOpenMark > parent.StartOpenMark && t.StartOpenMark < StartOpenMark 
                        && t.EndCloseMark < parent.EndCloseMark && t.EndCloseMark > EndCloseMark)
                    parent = t;
            }

            return parent;
        }
    }
}
