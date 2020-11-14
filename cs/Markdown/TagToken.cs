using FluentAssertions.Events;

namespace Markdown
{
    public class TagToken
    {
        public int StartPosition;
        public int ValueLength;
        public TagType Type;
        public int TagSignLength;
        public int EndPosition;
        public bool IsPaired;

        public TagToken(int startPosition, int endPosition, TagType type, bool isPaired = false)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Type = type;
            TagSignLength = TagAnalyzer.GetSignLength(type);
            ValueLength = (type is TagType.Shield) ? 0 : endPosition - startPosition - TagSignLength;
            IsPaired = isPaired;
        }
    }
}