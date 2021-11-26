namespace Markdown.MdTags
{
    public class TitleTag : Tag
    {
        public override TagType Type => TagType.Title;

        public TitleTag(int start, int end) : base(start, end)
        {
        }
    }
}