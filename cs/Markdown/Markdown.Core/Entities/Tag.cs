namespace Markdown.Core.Entities
{
    public class Tag
    {
        public string Value { get; }
        public TagType TagType { get; }

        public Tag(string value, TagType tagType)
        {
            Value = value;
            TagType = tagType;
        }
    }
}
