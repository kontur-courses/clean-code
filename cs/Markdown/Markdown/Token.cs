using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Length => End - Start + 1;
        public List<MarkupType> MarkupTypes { get; }

        public Token(int start, int end)
        {
            Start = start;
            End = end;
            MarkupTypes = new List<MarkupType>();
        }
    }
}