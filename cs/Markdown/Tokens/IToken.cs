using Markdown.Tags;

namespace Markdown.Tokens
{
    public interface IToken
    {
        public Tag TagType { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public string Value { get; }
        public int Length => EndPosition - StartPosition + 1;
    }
}