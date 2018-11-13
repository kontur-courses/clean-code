using System.Collections.Generic;

namespace Markdown
{
    internal sealed class StringToken : Token
    {
        public StringToken(int position, int length, string value)
        {
            Position = position;
            Length = length;
            Value = value;
        }

        public override int Position { get; set; }
        public override int Length { get; set; }
        public override string Value { get; set; }
        public override List<Token> InnerTokens { get; set; }
        public override Token ParentToken { get; set; }
        public override string ToHtml() => Value;
    }
}