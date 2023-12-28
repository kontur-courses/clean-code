namespace Markdown.Tags;

public interface ITag
{
    public string MdTag { get; }
    public string HtmlTag { get; }
    public bool IsOpenedCorrectly((char Left, char Right) adjacentSymbols);
    public bool IsClosedCorrectly((char Left, char Right) adjacentSymbols);
}