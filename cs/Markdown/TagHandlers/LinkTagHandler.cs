namespace Markdown.TagHandlers;

public class LinkTagHandler: BaseTagHandler, ITagHandler
{
    public LinkTagHandler(): base("[...](...)")
    {
        NestedTagHandlers = new ITagHandler[]
        {

        };
    }
    
    public string HtmlTag => $"<a>";

    protected override ITagHandler[] NestedTagHandlers { get; }
    public override bool StartsWithTag(string s, int startIndex)
    {
        return s[startIndex] == Tag[0];
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

    protected override string Format(string s) // [..](link) 
    {
        if (!IsValid(s))
            throw new ArgumentException();
        var parts = s.Split(']');
        if (parts.Length != 2) throw new FormatException();
        var text = parts[0][1..];
        var link = parts[1][..^1];
        return $"<a href=\"${link}\">{text}</a>";
    }
}