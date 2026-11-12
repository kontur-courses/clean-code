namespace Markdown.Nodes.HtmlTags;

public static class HtmlTagExtensions
{
    public static string HtmlToString(this HtmlTag tag)
    {
        return tag switch
        {
            HtmlTag.Em => "em",
            HtmlTag.Strong => "strong",
            HtmlTag.H1 => "h1",
            HtmlTag.Ul => "ul",
            HtmlTag.Li => "li",
            _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, "Unknown HTML tag")
        };
    }
}