namespace Markdown.TagHandlers;

public class HeadingHandler : BaseTagHandler, ITagHandler
{
    public HeadingHandler() : base("#", "<h1>")
    {
        NestedTagHandlers = new ITagHandler[]
        {
            new BoldTagHandler(),
            new ItalicTagHandler(),
            new LinkTagHandler()
        };
    }

    protected override ITagHandler[] NestedTagHandlers { get; }

    public override bool IsValid(string text, int startIndex = 0)
    {
        if (string.IsNullOrWhiteSpace(text)
            || startIndex < 0 || startIndex >= text.Length
            || (startIndex != 0 && text[startIndex - 1] != '\n'))
            return false;

        return StartsWithTag(text, startIndex) && char.IsWhiteSpace(text[startIndex + 1]);
    }

    public override int FindEndTagProcessing(string text, int startIndex)
    {
        ValidateInput(text, startIndex);
        var end = text.IndexOf('\n', startIndex + 1);
        return end != -1 ? end : text.Length;
    }

    protected override string GetInnerContent(string s, int startIndex)
    {
        ValidateInput(s, startIndex);
        var end = FindEndTagProcessing(s, startIndex);
        var newIndex = startIndex + 1;
        return s[newIndex..end].Trim();
    }

    protected override string Format(string s)
    {
        var closingTag = HtmlTag.Insert(1, "/");
        return $"{HtmlTag}{GetInnerContent(s, 0).Trim()}{closingTag}";
    }
}