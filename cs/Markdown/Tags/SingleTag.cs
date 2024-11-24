namespace Markdown.Tags;

public abstract class SingleTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlTag { get; }

    public virtual bool IsOpenedCorrectly((char left, char right) adjacentChars) =>
        adjacentChars.left is '\n' or '\0';

    public virtual bool IsClosedCorrectly((char left, char right) adjacentChars) => true;
}