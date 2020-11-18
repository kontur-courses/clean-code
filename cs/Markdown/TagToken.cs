namespace Markdown
{
    public class TagToken
    {
        public readonly int StartPosition;
        public readonly int ValueLength;
        public readonly TagType Type;
        public int TagSignLength => TagAnalyzer.GetSignLength(Type);
        public readonly int EndPosition;

        public TagToken(int startPosition, int endPosition, TagType type)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Type = type;
            ValueLength = (type is TagType.Shield) ? 0 : endPosition - startPosition - TagSignLength;
        }
    }
}