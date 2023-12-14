namespace Markdown.Tags;

public class Tag : IConvertableToString
{
    public Tag(string value, TagType tagType)
    {
        Value = value;
        TagType = tagType;
    }

    public TagType TagType { get; }
    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }
}