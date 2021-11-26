namespace Markdown.MdTags
{
    public class ListElement : Tag
    {
        public override TagType Type => TagType.ListElement;

        public ListElement(int start, int end) : base(start, end)
        {
        }
    }
}