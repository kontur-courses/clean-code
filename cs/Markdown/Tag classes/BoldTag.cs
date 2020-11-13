namespace Markdown
{
    public class BoldTag : Tag
    {
        public BoldTag(int startOfOpeningTag, int startOfClosingTag)
            : base(startOfOpeningTag, startOfClosingTag)
        {
        }

        public BoldTag()
        {
        }

        public override string MdTag => "__";
        public override string HtmlTagName => "strong";
    }
}