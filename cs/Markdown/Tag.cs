namespace Markdown
{
    public enum TagType
    {
        Open,
        Close
    }
    
    public class Tag
    {
        public string Value { get; }
        public TagType Type { get; }

        public Tag(string value, TagType type)
        {
            Value = value;
            type = type;
        }
    }
}