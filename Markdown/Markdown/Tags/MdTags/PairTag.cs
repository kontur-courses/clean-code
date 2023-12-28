namespace Markdown;

public abstract class PairTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlTag { get; }
    protected virtual IEnumerable<PairTag> ProhibitedInside => Array.Empty<PairTag>();
    public bool CanContain(ITag tag) => ProhibitedInside.All(x => x.GetType() != tag.GetType());
    public virtual bool IsOpenedCorrectly((char Left, char Right) adjacentSymbols) => adjacentSymbols.Right != ' ';
    public virtual bool IsClosedCorrectly((char Left, char Right) adjacentSymbols) => adjacentSymbols.Left != ' ';
}