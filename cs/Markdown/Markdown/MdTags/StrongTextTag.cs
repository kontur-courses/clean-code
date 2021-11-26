namespace Markdown.MdTags
{
    public class StrongTextTag : Tag
    {
        public override TagType Type => TagType.StrongText;

        public StrongTextTag(int start, int end) : base(start, end)
        {
        }
    }
}