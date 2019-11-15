namespace Markdown
{
    enum TagType
    {
        Opening,
        Closing,
    }

    class Tag
    {
        public readonly string Symbol;
        public readonly int Index;
        public readonly TagType Type;

        public Tag(string symbol, int index, TagType type)
        {
            Symbol = symbol;
            Index = index;
            Type = type;
        }
    }
}
