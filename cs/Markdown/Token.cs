using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public readonly int StartIndex;
        public readonly TagInfo TagInfo;

        public Token(int startIndex, int length, TagInfo tagInfo)
        {
            StartIndex = startIndex;
            Length = length;
            TagInfo = tagInfo;
        }
    }
}