using MarkDown.Enums;
using MarkDown.TagContexts.Abstracts;

namespace MarkDown.Tags.Abstracts;

public abstract class TagFactory : IComparable<TagFactory>
{
    protected readonly MarkDownEnvironment Environment;

    protected TagFactory(MarkDownEnvironment environment)
    {
        Environment = environment;

        unchecked
        {
            hashCode = 11;

            hashCode = (hashCode * 397) ^ HtmlOpen.GetHashCode();
            hashCode = (hashCode * 397) ^ HtmlClose.GetHashCode();
            hashCode = (hashCode * 397) ^ MarkDownOpen.GetHashCode();
            hashCode = (hashCode * 397) ^ MarkDownClose.GetHashCode();
        }
    }
    
    public abstract TagName TagName { get; }
    public abstract string HtmlOpen { get; }
    public abstract string HtmlClose { get; }
    public abstract string MarkDownOpen { get; }
    public abstract string MarkDownClose { get; }
    
    private readonly int hashCode;

    public abstract TagContext CreateContext(string mdText, int startIndex, TagContext parentContext);

    public abstract bool CanCreateContext(string text, int position);

    public abstract bool IsClosePosition(string text, int position);

    public override int GetHashCode()
    {
        return hashCode;
    }

    public int CompareTo(TagFactory? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        
        var htmlOpenComparison = string.Compare(HtmlOpen, other.HtmlOpen, StringComparison.Ordinal);
        if (htmlOpenComparison != 0) return htmlOpenComparison;
        
        var htmlCloseComparison = string.Compare(HtmlClose, other.HtmlClose, StringComparison.Ordinal);
        if (htmlCloseComparison != 0) return htmlCloseComparison;
        
        var markDownOpenComparison = string.Compare(MarkDownOpen, other.MarkDownOpen, StringComparison.Ordinal);
        if (markDownOpenComparison != 0) return markDownOpenComparison;
        
        return string.Compare(MarkDownClose, other.MarkDownClose, StringComparison.Ordinal);
    }
}