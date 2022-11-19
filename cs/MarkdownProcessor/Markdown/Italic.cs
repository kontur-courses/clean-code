namespace MarkdownProcessor.Markdown;

public class Italic : IMarkdownTag
{
    public Italic(IMarkdownTag parent, int startIndex)
    {
        Parent = parent;
        StartIndex = startIndex;
    }

    public static TextType Type => TextType.Italic;
    public static string MarkdownSign => "_";
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