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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Style))
                return false;
            var other = (Style) obj;
            return StartTag == other.StartTag && EndTag == other.EndTag && Type == other.Type;
        }

        public override int GetHashCode()
        {
            return StartTag.GetHashCode() + 3 * EndTag.GetHashCode() + 5 * Type.GetHashCode();
        }
    }

    public enum StyleType
    {
        Heading,
        Italic,
        Bold,
        UnorderedList,
        UnorderedListElement
    }
}