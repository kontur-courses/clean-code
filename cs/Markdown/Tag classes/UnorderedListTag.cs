namespace Markdown
{
    public class UnorderedListTag : Tag
    {
        public UnorderedListTag(int startOfOpeningTag, int startOfClosingTag)
            : base(startOfOpeningTag, startOfClosingTag)
        {
        }

        public UnorderedListTag()
        {
        }

        public override string MdTag => "*";
        public override string HtmlTagName => "li";
    }
}