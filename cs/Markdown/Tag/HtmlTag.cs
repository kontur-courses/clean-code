namespace Markdown.Tag;

public class HtmlTag
{
    private readonly string markup;

    public HtmlTag(TagType type, int index, bool isClosing, string htmlMarkup)
    {
        this.Type = type;
        this.Index = index;
        this.IsClosing = isClosing;
        markup = htmlMarkup;
    }

    public TagType Type { get; }

    public int Index { get; }

    public bool IsClosing { get; }

    public string GetMarkup()
    {
        if (Type == TagType.EscapedSymbol)
            return "";
        
        return IsClosing ? $"</{markup}>" : $"<{markup}>";
    }

}
    