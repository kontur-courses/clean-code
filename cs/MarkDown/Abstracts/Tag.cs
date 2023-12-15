using MarkDown.Interfaces;

namespace MarkDown.Abstracts;

public abstract class Tag
{
    protected readonly MarkDownEnvironment Environment;
    
    public Tag(MarkDownEnvironment environment)
    {
        Environment = environment;
    }
    
    public abstract string HtmlSyntax { get; }
    public abstract string MarkDownOpen { get; }
    public abstract string MarkDownClose { get; }
    public abstract bool CanCancelContext { get; }

    public abstract ITagContext CreateContext(int startIndex);
}