namespace Markdown
{
    class Substring
    {
        public Substring(int index, string value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; }

        public string Value { get; }

        public int Length => Value.Length;
    }
}