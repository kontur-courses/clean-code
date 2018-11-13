using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class CharsGroup
    {
        private readonly char[] chars;
        private readonly bool isExcludeChars;

        public int Index { get; }

        public CharsGroup(char[] chars, bool isExcludeChars = false, int index)
        {
            this.chars = chars;
            this.isExcludeChars = isExcludeChars;
            Index = index;
        }

        public bool Contains(char ch)
        {
            if (isExcludeChars)
                return !chars.Contains(ch);
            return chars.Contains(ch);
        }
    }
}
