using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags
{
    public abstract class PairedTag : Tag
    {
        protected PairedTag Pair { get; set; }
        protected readonly TagPosition Position;
        protected PairedTag(TagPosition tagPosition, string data) : base(data)
        {
            Position = tagPosition;
        }
    }
}
