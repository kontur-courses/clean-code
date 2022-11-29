namespace Markdown.Primitives;

public record Tag(TagType Type, string Value)
{
    public static string GetName(TagType type)
    {
        return type switch
        {
            TagType.Header1 => "h1",
            TagType.Italic => "em",
            TagType.Bold => "strong",
            TagType.Link => "a",
            _ => throw new ArgumentException($"Unsupported {type} tag")
        };
    }
};