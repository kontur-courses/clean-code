namespace Markdown.Tags;

public abstract class BoundaryTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlOpenTag { get; }
    public abstract string HtmlCloseTag { get; }

    public virtual bool CanBeOpened(char left, char right)
    {
        return !char.IsWhiteSpace(right) && !char.IsDigit(left);
    }

    public virtual bool CanBeClosed(char left, char right)
    {
        return !char.IsWhiteSpace(left) && !char.IsDigit(left);
    }

    protected virtual IReadOnlyList<Type> InvalidInnerTags => [];
    public bool CanBeInnerTag(ITag tag) => InvalidInnerTags.Contains(tag.GetType());
}