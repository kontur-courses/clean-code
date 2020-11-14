namespace Markdown
{
    public class Tag
    {
        public readonly string Value;
        public readonly int Index;

        public Tag(string value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}