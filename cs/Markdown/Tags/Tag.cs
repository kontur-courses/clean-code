namespace Markdown.Tags
{
    public class Tag
    {
        public string Value { get; }
        public int Index { get; }
        public int Length { get; }

        public Tag(string value, int index, int length)
        {
            Value = value;
            Index = index;
            Length = length;
        }
    }
}