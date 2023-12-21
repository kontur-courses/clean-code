namespace Markdown.Tags.TextTag;

public class Tag : IConvertableToString
{
    public Tag(string value, TagStatus tagType)
    {
        Value = value;
        TagStatus = tagType;
    }

    public TagStatus TagStatus { get; set; }
    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }
}