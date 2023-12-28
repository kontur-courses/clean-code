namespace Markdown;

public abstract class SingleTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlTag { get; }
    public virtual bool IsOpenedCorrectly((char Left, char Right) adjacentSymbols) => true;
    public virtual bool IsClosedCorrectly((char Left, char Right) adjacentSymbols) => true;
}