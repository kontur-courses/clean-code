namespace Markdown.Nodes;

public static class HtmlTagExtensions
{
    public static string HtmlToString(this HtmlTag tag)
    {
        return tag switch
        {
            HtmlTag.Em => "em",
            HtmlTag.Strong => "strong",
            HtmlTag.H1 => "h1",
            _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, "Unknown HTML tag")
        };
    }
}