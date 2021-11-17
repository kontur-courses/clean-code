namespace Markdown.Tags
{
    public class Tag : ITag
    {
        public string Value { get; set; }
        public TagType Type  { get; set; }

        public Tag(string value, TagType type)
        {
            Value = value;
            Type = type;
        }
    }
}