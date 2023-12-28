namespace Markdown.Tags.MdTags;

public abstract class SingleTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlTag { get; }
    public abstract string? HtmlContainer { get; }
    public virtual bool IsOpenedCorrectly((char Left, char Right) adjacentSymbols) => adjacentSymbols.Left is '\n' or '\0';
    public virtual bool IsClosedCorrectly((char Left, char Right) adjacentSymbols) => true;
}