using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public int Start { get; set; }

        public int Length { get; set; }
        public List<MarkupType> MarkupTypes { get; }

        public Token(int start, int length)
        {
            Start = start;
            Length = length;
            MarkupTypes = new List<MarkupType>();
        }
    }
}