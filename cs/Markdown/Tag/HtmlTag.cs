namespace Markdown.Tag;

public class HtmlTag
{
    private readonly string tag;

    public HtmlTag(TagType type, int index, bool isClosing, string htmlTag)
    {
        Type = type;
        Index = index;
        IsClosing = isClosing;
        tag = htmlTag;
    }

    public TagType Type { get; }

    public int Index { get; }

    public bool IsClosing { get; }

    public string GetTag()
    {
        if (Type == TagType.EscapedSymbol)
            return "";
        
        return IsClosing ? $"</{tag}>" : $"<{tag}>";
    }

}
    