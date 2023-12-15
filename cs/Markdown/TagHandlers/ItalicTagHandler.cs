namespace Markdown.TagHandlers;

public class ItalicTagHandler: BaseTagHandler, ITagHandler
{
    public ItalicTagHandler() : base("_")
    {
        NestedTagHandlers = new ITagHandler[]
        {
            
        };
    }

    protected override ITagHandler[] NestedTagHandlers { get; }

    public override bool StartsWithTag(string s, int startIndex)
    {
        return s[startIndex..].StartsWith(Tag);

    }

    public override bool IsValid(string s, int startIndex = 0)
    {
        throw new NotImplementedException();
    }

    public override int FindEndTagProcessing(string s, int startIndex)
    {
        throw new NotImplementedException();
    }

    protected override string GetInnerContent(string s, int startIndex)
    {
        throw new NotImplementedException();
    }

    protected override string Format(string s)
    {
        throw new NotImplementedException();
    }
}