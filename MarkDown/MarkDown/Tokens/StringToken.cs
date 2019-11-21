using MarkDown.TagParsers;

namespace MarkDown.Tokens
{
    public class StringToken : MdToken
    {
        public override string Value { get; }
        public override int Length { get; }
        public override TokenTypes TokenType { get; protected set; } = TokenTypes.String;

        public StringToken(string value, int length)
        {
            Value = value;
            Length = length;
        }
    }
}