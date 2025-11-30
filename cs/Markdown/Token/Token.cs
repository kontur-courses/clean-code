namespace Markdown;

public class Token(TagType tagType, string content, List<Token>? children = null)
{
    public TagType TagType { get; } = tagType;

    public string Content { get; } = content;

    public List<Token>? Children { get; } = children;
}