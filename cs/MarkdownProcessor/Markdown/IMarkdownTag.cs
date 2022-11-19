namespace MarkdownProcessor.Markdown;

public interface IMarkdownTag
{
    public static TextType Type { get; }
    public static string MarkdownSign { get; }
    public int StartIndex { get; }
    public int EndIndex { get; }
    public IMarkdownTag Parent { get; }
    public IEnumerable<IMarkdownTag> Children { get; }
    public bool Closed { get; }
    public bool TryCreate(IMarkdownTag tree);

    public int RunSymbolDownOfTree();
    public int RunTagDownOfTree();
}