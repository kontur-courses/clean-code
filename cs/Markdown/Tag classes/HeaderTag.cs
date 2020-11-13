namespace Markdown
{
    public class HeaderTag : Tag
    {
        public HeaderTag(int startOfOpeningTag, int startOfClosingTag)
            : base(startOfOpeningTag, startOfClosingTag)
        {
        }

        public HeaderTag()
        {
        }

        public override string MdTag => "#";
        public override string HtmlTagName => "h1";
    }
}