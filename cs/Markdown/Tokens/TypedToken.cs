using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TypedToken : Token
    {
        public TypedToken(int start, int length, TokenType type, TagType tagType = TagType.Undefined,
            SubTagOrder order = SubTagOrder.Undefined)
            : base(start, length)
        {
            Type = type;
            TagType = tagType;
            Order = order;
        }

        public TypedToken(ITag tag, SubTagOrder order, int start)
            : base(start, tag.GetSubTag(order).Length)
        {
            Type = TokenType.Tag;
            TagType = tag.Type;
            Order = order;
        }

        public TokenType Type { get; set; }

        public SubTagOrder Order { get; set; }

        public TagType TagType { get; set; }

        public void SwitchToTextToken()
        {
            Type = TokenType.Text;
            TagType = TagType.Undefined;
            Order = SubTagOrder.Undefined;
        }
    }
}