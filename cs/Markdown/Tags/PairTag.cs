namespace Markdown.Tags;

public abstract class PairTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlTag { get; }

    public virtual bool IsOpenedCorrectly((char left, char right) adjacentChars) =>
        adjacentChars.right != ' ';
    public virtual bool IsClosedCorrectly((char left, char right) adjacentChars) =>
        adjacentChars.left != ' ';
    
    protected virtual IEnumerable<PairTag> ProhibitedInside => [];
}