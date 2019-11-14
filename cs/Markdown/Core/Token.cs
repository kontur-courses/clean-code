using Markdown.Core.Tags;

namespace Markdown.Core
{
    class Token
    {
        public int StartPosition { get; }
        public ITag Tag { get; }
        public string Value { get; }
        public int Length => Value.Length;
        public bool IsOpening { get; }

        public Token(int startPosition, ITag tag, bool isOpening)
        {
            StartPosition = startPosition;
            Value = isOpening ? tag.Opening : tag.Closing;
            Tag = tag;
            IsOpening = isOpening;
        }
    }
}