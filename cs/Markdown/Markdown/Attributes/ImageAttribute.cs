namespace Markdown.Markdown.Attributes;

public class ImageAttribute(AttributeType type, string value) : IAttribute
{
    public AttributeType Type { get; } = type;
    public string Value { get; } = value;
}