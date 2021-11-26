namespace Markdown.MdTags
{
    public class UnnumberedList : Tag
    {
        public override TagType Type => TagType.UnnumberedList;

        public UnnumberedList(int start, int end) : base(start, end)
        {
        }
    }
}