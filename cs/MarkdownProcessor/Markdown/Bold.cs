namespace MarkdownProcessor.Markdown;

public class Bold : IMarkdownTag
{
    public Bold(IMarkdownTag parent, int startIndex)
    {
        Parent = parent;
        StartIndex = startIndex;
    }

    public static TextType Type => TextType.Bold;
    public static string MarkdownSign => "__";
    public int StartIndex { get; }
    public int EndIndex { get; } = 0;
    public IMarkdownTag Parent { get; }
    public IEnumerable<IMarkdownTag> Children { get; } = new List<IMarkdownTag>();
    public bool Closed { get; } = false;

    public bool TryCreate(IMarkdownTag tree)
    {
        throw new NotImplementedException();
    }

    public int RunSymbolDownOfTree()
    {
        throw new NotImplementedException();
    }

    public int RunTagDownOfTree()
    {
        throw new NotImplementedException();
    }
}