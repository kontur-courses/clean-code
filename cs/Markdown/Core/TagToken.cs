using Markdown.Core.Tags;

namespace Markdown.Core
{
    class TagToken : Token
    {
        public ITag Tag { get; }
        public bool IsOpening { get; }

        public TagToken(int startPosition, ITag tag, bool isOpening) : base(startPosition,
            isOpening ? tag.Opening : tag.Closing)
        {
            Tag = tag;
            IsOpening = isOpening;
        }
    }
}