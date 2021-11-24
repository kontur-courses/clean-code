namespace Markdown.Tags
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

        public static Tag Cursive(string value) => new(TagType.Cursive, value);

        public static Tag Bold(string value) => new(TagType.Bold, value);

        public static Tag Header1(string value) => new(TagType.Header1, value);
    }
}