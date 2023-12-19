namespace Markdown.Tags.TextTag;

public class Tag : IConvertableToString
{
    public Tag(string value, TagType tagType)
    {
        Value = value;
        TagType = tagType;
    }

    public TagType TagType { get; set; }
    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }
}