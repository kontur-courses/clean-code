namespace MarkdownProcessor.Markdown;

public class FirstHeader : IMarkdownTag
{
    public FirstHeader(IMarkdownTag parent, int startIndex)
    {
        Parent = parent;
        StartIndex = startIndex;
    }

    public static TextType Type => TextType.FirstHeader;
    public static string MarkdownSign => "# ";
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