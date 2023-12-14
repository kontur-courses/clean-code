using Markdown.Interfaces;

namespace Markdown.Tags;

public class Tag: IConvertableToString
{
    public TagType TagType { get; }
    public string Value { get; }

    public Tag(string value, TagType tagType)
    {
        Value = value;
        TagType = tagType;
    }

    public override string ToString()
    {
        return Value;
    }
}