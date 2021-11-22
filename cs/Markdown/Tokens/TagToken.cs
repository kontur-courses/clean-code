using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : IToken
    {
        public Tag TagType { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public string Value { get; }

        public TagToken(Tag tagType, int startPosition, int endPosition, string value)
        {
            TagType = tagType;
            StartPosition = startPosition;
            EndPosition = endPosition;
            Value = value;
        }

        public bool IsNestedInToken(IToken otherToken)
        {
            return StartPosition > otherToken.StartPosition && EndPosition < otherToken.EndPosition;
        }
    }
}