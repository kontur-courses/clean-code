using Markdown.Core.Tags;

namespace Markdown.Core
{
    class Token
    {
        public int StartPosition { get; }
        public string Value { get; }
        public int Length => Value.Length;

        public Token(int startPosition, string value)
        {
            StartPosition = startPosition;
            Value = value;
        }
    }
}