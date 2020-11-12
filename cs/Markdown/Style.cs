namespace Markdown
{
    public class Style
    {
        public readonly string EndTag;
        public readonly string StartTag;
        public readonly StyleType Type;

        public Style(StyleType type, string startTag, string endTag)
        {
            Type = type;
            StartTag = startTag;
            EndTag = endTag;
        }
    }

    public enum StyleType
    {
        Heading,
        Italic,
        Bold
    }
}