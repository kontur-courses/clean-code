namespace Markdown
{
    using System.Collections.Generic;

    public sealed class PairedTagToken : Token
    {
        public PairedTagToken(int position, int length, string value, string fullTag = null)
        {
            Position = position;
            Length = length;
            Value = value;
            FullTag = fullTag;
        }

        public string Content => Value.Substring(FullTag.Length, Value.Length - 2 * FullTag.Length);

        public string FullTag { get; set; }

        public override List<Token> InnerTokens { get; set; }

        public override int Length { get; set; }

        public override Token ParentToken { get; set; }

        public override int Position { get; set; }

        public override string Value { get; set; }
    }
}
