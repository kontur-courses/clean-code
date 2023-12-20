namespace Markdown;

public interface ITag
{
    public string MdTag { get; }
    public bool IsPaired { get; }
    public HtmlTag HtmlTag { get; }
}