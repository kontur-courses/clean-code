using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class TaggedToken : Token
    {
        public Tag Tag { get; }
        public TaggedToken(string value, int startPosition, Tag tag) :
            base(value, startPosition)
        {
            Tag = tag;
        }
    }
}
