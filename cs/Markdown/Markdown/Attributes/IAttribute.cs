namespace Markdown.Markdown.Attributes;

public interface IAttribute
{
    AttributeType Type { get; }
    string Value { get; }
}