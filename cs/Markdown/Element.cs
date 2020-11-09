using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Element
    {
        public readonly Style ElementStyle;
        public readonly int ElementStart;
        public readonly int ElementLength;
        public readonly int ContentStart;
        public readonly int ContentLength;

        public Element(Style style, int elementStart, int elementLength, int contentStart, int contentLength) 
        {
            ElementStyle = style;
            ElementStart = elementStart;
            ElementLength = elementLength;
            ContentStart = contentStart;
            ContentLength = contentLength;
        }
    }
}
