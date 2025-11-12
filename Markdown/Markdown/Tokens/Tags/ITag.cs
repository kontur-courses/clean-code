namespace Markdown.Tokens.Tags;

public interface ITag
{
    public ETagType Type { get; }
    public string HtmlTag { get; }
}