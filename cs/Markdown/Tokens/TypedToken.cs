using System.Runtime.CompilerServices;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TypedToken : Token
    {
        public TokenType Type { get; set; }

        public SubTagOrder Order { get; set; }

        public TagType TagType { get; set; }

        public TypedToken(int start, int length, TokenType type, TagType tagType = TagType.Undefined, SubTagOrder order = SubTagOrder.Undefined)
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

        public void SwitchToTextToken()
        {
            this.Type = TokenType.Text;
            this.TagType = TagType.Undefined;
            this.Order = SubTagOrder.Undefined;
        }
    }
}
