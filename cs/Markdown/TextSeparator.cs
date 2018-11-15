namespace Markdown
{
    public struct TextSeparator
    {
        public string Separator { get; }
        public int Index { get; }

        public TextSeparator(string separator, int index)
        {
            Separator = separator;
            Index = index;
        }
    }
}
