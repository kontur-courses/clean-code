using Markdown.Core.Tags;

namespace Markdown.Core
{
    public class TagToken : Token
    {
        public ITag Tag { get; }
        public bool IsOpening { get; }

        public TagToken(int startPosition, ITag tag, string value, bool isOpening) : base(startPosition,
            value)
        {
            Tag = tag;
            IsOpening = isOpening;
        }
    }
}