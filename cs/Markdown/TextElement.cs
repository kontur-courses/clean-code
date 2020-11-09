using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public struct TextElement //возможно стоит назвать HTMLElement
    {
        public int StartPosition;
        public int Length;
        public TextElementType Type;

        public TextElement(int startPosition, int length, TextElementType type)
        {
            StartPosition = startPosition;
            Length = length;
            Type = type;
        }
    }
}
