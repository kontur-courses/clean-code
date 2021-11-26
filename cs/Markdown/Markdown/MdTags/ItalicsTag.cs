namespace Markdown.MdTags
{
    public class ItalicsTag : Tag
    {
        public override TagType Type => TagType.Italics;

        public ItalicsTag(int start, int end) : base(start, end)
        {
        }
    }
}