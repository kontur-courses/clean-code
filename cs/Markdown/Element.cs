using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Element
    {
        public Style ElementStyle;
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

        public static Element Create(Style style, int startTagPosition, int endTagPosition)
        {
            return new Element(
                style,
                startTagPosition,
                endTagPosition + style.EndTag.Length - startTagPosition,
                startTagPosition + style.StartTag.Length,
                endTagPosition - startTagPosition - style.StartTag.Length);
        }
    }
}
