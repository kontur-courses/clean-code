namespace Markdown
{
    public class TagToken
    {
        public int StartPosition;
        public int ValueLength;
        public TagType Type;
        public int TagSignLength => TagAnalyzer.GetSignLength(Type);
        public int EndPosition;

        public TagToken(int startPosition, int endPosition, TagType type)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Type = type;
            ValueLength = (type is TagType.Shield) ? 0 : endPosition - startPosition - TagSignLength;
        }
    }
}