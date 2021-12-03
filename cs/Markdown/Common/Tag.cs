namespace Markdown.Common
{
    public class Tag
    {
        public int Position { get; }
        public BaseMdTag MdTagType { get; }

        public Tag(int position, BaseMdTag mdTagType)
        {
            Position = position;
            MdTagType = mdTagType;
        }
    }
}