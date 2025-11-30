namespace Markdown;

public class TokenTagLink(string content, string linkText, string? tooltipText = null, List<Token>? children = null) : Token(TagType.Link, content, children)
{
    public string? TooltipText { get; } = tooltipText;
    public string LinkText { get; } = linkText;
}