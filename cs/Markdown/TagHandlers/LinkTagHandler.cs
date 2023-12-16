namespace Markdown.TagHandlers;

public class LinkTagHandler : BaseTagHandler, ITagHandler
{
    public LinkTagHandler() : base("[]()", "<a>")
    {
        NestedTagHandlers = Array.Empty<ITagHandler>();
    }

    protected override ITagHandler[] NestedTagHandlers { get; }

    public override bool StartsWithTag(string text, int startIndex)
    {
        return text[startIndex] == MdTag[0];
    }

    public override bool IsValid(string text, int startIndex = 0)
    {
        throw new NotImplementedException();
    }

    public override int FindEndTagProcessing(string text, int startIndex)
    {
        throw new NotImplementedException();
    }

    protected override string GetInnerContent(string s, int startIndex)
    {
        throw new NotImplementedException();
    }

    protected override string Format(string s)
    {
        ValidateInput(s, 0);
        var parts = s.Split(']');
        var text = parts[0][1..];
        var link = parts[1][..^1];
        return $"<a href=\"${link}\">{text}</a>";
    }
}