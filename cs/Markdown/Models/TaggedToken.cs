using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class TaggedToken : Token
    {
        public Tag Tag { get; }
        public TaggedToken(string value, Tag tag) :
            base(value)
        {
            Tag = tag;
        }
    }
}
