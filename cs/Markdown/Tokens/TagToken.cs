using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : IToken
    {
        public Tag Tag { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public string Value { get; }

        public TagToken(Tag tag, int startPosition, int endPosition, string value)
        {
            Tag = tag;
            StartPosition = startPosition;
            EndPosition = endPosition;
            Value = value;
        }

        public bool IsNestedInAnotherToken(IToken anotherToken)
        {
            return StartPosition > anotherToken.StartPosition && EndPosition < anotherToken.EndPosition;
        }
    }
}