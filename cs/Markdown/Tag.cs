namespace Markdown
{
    public class Tag
    {
        public readonly string Value;
        public readonly int Index;
        public readonly int Length;

        public Tag(string value, int index, int length)
        {
            Value = value;
            Index = index;
            Length = length;
        }
    }
}