using System.Collections.Generic;

namespace Markdown
{
    public sealed class UnderscoreToken : Token
    {
        public UnderscoreToken(int position, int length, string value)
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
        public override string ToHtml()
        {
            return Value[1] == '_' ? $"<strong>{Value.Substring(2, Value.Length - 3)}</strong>" : $"<em>{Value.Substring(1, Value.Length - 2)}</em>";
        }
    }
}