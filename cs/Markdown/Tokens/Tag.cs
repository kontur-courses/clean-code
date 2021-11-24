namespace Markdown.Tokens
{
    public readonly struct Tag : ITextContainer
    {
        public readonly TagType Type;
        public string Value { get; }

        private Tag(TagType type, string value)
        {
            Type = type;
            Value = value;
        }

        public static Tag Text(string value) => new(TagType.Text, value);

        public static Tag Cursive = new(TagType.Cursive, Token.Cursive.Value);

        public static Tag Bold = new(TagType.Bold, Token.Bold.Value);

        public static Tag Header1 = new(TagType.Header1, Token.Header1.Value);
    }
}