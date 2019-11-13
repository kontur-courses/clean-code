namespace Markdown
{
    enum SeparatorType
    {
        Opening,
        Closing,
    }

    class Separator
    {
        public readonly string Tag;
        public readonly int Index;
        public readonly SeparatorType Type;

        public Separator(string tag, int index, SeparatorType type)
        {
            Tag = tag;
            Index = index;
            Type = type;
        }
    }
}
