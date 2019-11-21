using MarkDown.TagParsers;

namespace MarkDown
{
    public abstract class MdToken
    {
        public abstract string Value { get; }
        public abstract int Length { get; }
        public abstract TokenTypes TokenType { get; protected set; }
    }
}