namespace Markdown;

public interface ITag
{
    public string HtmlTag { get; }
    public bool IsPaired { get; }
    public string OpenTag { get; }
    public string CloseTag { get; }
}