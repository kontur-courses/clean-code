namespace Markdown
{
    public class Element
    {
        public readonly int ContentLength;
        public readonly int ContentStart;
        public readonly int ElementLength;
        public readonly int ElementStart;
        public Style ElementStyle;

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

        public bool IntersectsWith(Element otherElement)
        {
            return ElementStart > otherElement.ElementStart
                   && ElementStart < otherElement.ElementStart + otherElement.ElementLength
                   && ElementStart + ElementLength > otherElement.ElementStart + otherElement.ElementLength
                   || ElementStart < otherElement.ElementStart
                   && ElementStart + ElementLength > otherElement.ElementStart
                   && ElementStart + ElementLength < otherElement.ElementStart + otherElement.ElementLength;
        }
    }
}