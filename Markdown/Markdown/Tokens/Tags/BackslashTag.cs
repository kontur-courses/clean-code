namespace Markdown.Tokens.Tags;

public class BackslashTag : ITag
{
    public ETagType Type => ETagType.Escape;
    public string HtmlTag { get; }
}