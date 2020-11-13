namespace Markdown
{
    public class ItalicTag : Tag
    {
        public ItalicTag(int startOfOpeningTag, int startOfClosingTag)
            : base(startOfOpeningTag, startOfClosingTag)
        {
        }

        public ItalicTag()
        {
        }

        public override string MdTag => "_";
        public override string HtmlTagName => "em";
    }
}