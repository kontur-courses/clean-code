using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags
{
    public abstract class PairedTag : Tag
    {
        public PairedTag Pair { get; protected set; }
        protected TagPosition Position;
        protected PairedTag(TagPosition tagPosition, string data) : base(data)
        {
            Position = tagPosition;
        }
    }
}
