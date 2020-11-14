namespace Markdown
{
    public class TagReplacementInfo
    {
        public int Position;
        public TagType Type;
        public bool IsCloser;
        public bool IsPaired;

        public TagReplacementInfo(int position, TagType type, bool isCloser, bool isPaired)
        {
            Position = position;
            Type = type;
            IsCloser = isCloser;
            IsPaired = isPaired;
        }
    }
}