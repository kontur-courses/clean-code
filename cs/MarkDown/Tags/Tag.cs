using MarkDown.Enums;
using MarkDown.Interfaces;

namespace MarkDown.Tags;

public abstract class Tag
{
    protected readonly MarkDownEnvironment Environment;
    
    public Tag(MarkDownEnvironment environment)
    {
        Environment = environment;
    }
    
    public abstract TagName TagName { get; }
    
    public abstract string HtmlOpen { get; }
    public abstract string HtmlClose { get; }
    public abstract string MarkDownOpen { get; }
    public abstract string MarkDownClose { get; }
    // protected abstract bool CanCancelContext { get; }

    public abstract TagContext CreateContext(int startIndex, TagContext parentContext);

    public abstract bool CanCreateContext(string text, int position);

    public abstract bool IsClosePosition(string text, int position);
}