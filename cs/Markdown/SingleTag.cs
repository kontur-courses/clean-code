namespace Markdown
{
    public class SingleTag: Tag
    {
        public SingleTag(TagType type, int start, bool isOpening = true) : base(type, start, isOpening)
        {
        }
    }
}