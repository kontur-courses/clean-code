namespace Markdown
{
    public class TagType
    {
        public readonly string HtmlOpeningTag;
        public readonly string HtmlClosingTag;
        public readonly string MdOpeningTag;
        public readonly string MdClosingTag;

        public TagType(string htmlOpeningTag, string htmlClosingTag, string mdOpeningTag, string mdClosingTag)
        {
            HtmlOpeningTag = htmlOpeningTag;
            HtmlClosingTag = htmlClosingTag;
            MdOpeningTag = mdOpeningTag;
            MdClosingTag = mdClosingTag;
        }
        
        public static TagType GetTagType(string text, int position)
        {
            if (EmTagType.IsOpenedTag(text, position) || EmTagType.IsClosedTag(text, position))
                return new EmTagType();
            if (StrongTagType.IsOpenedTag(text, position) || StrongTagType.IsClosedTag(text, position))
                return new StrongTagType();
            return new DefaultTagType();
        }
    }
}