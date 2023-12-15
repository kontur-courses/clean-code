namespace Markdown.TagHandlers;

public class HeadingHandler : BaseTagHandler, ITagHandler
{
    public HeadingHandler() : base("#")
    {
        NestedTagHandlers = new ITagHandler[]
        {
            new BoldTagHandler(),
            new ItalicTagHandler(),
            new LinkTagHandler()
        };
    }

    protected override ITagHandler[] NestedTagHandlers { get; }

    public override bool StartsWithTag(string s, int startIndex)
    {
        return s[startIndex..].StartsWith(Tag);
    }

    public override bool IsValid(string s, int startIndex = 0)
    {
        if (startIndex < 0 || startIndex >= s.Length)
            throw new ArgumentOutOfRangeException();
        if (string.IsNullOrWhiteSpace(s)) return false;
        if (startIndex != 0 && s[startIndex - 1] != '\n')
            return false;
        return StartsWithTag(s, startIndex) && char.IsWhiteSpace(s[startIndex + 1]);
    }

    public override int FindEndTagProcessing(string s, int startIndex)
    {
        if (!IsValid(s, startIndex))
            throw new ArgumentException();
        var end = s.IndexOf('\n', startIndex + 1);
        return end != -1
            ? end
            : s.Length;
    }

    protected override string GetInnerContent(string s, int startIndex)
    {
        if (!IsValid(s, startIndex))
            throw new ArgumentException();
        var end = FindEndTagProcessing(s, startIndex);
        var newIndex = startIndex + 1;
        return s[newIndex..end].Trim();
    }

    protected override string Format(string s)
    {
        return $"<h1>{GetInnerContent(s, 0).Trim()}</h1>";
    }
}