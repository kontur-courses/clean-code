using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.Elements;

public class ParagraphElement : IElement, IStorageOf<ItalicElement>, IStorageOf<PlainText>
{
    public string RawContent { get; }
    private readonly List<IElement> _nestedElements = new();
    public IEnumerable<IElement> NestedElements => _nestedElements;

    public ParagraphElement(string rawContent)
    {
        RawContent = rawContent;
    }

    public void AddNestedElement(IElement nested)
    {
        _nestedElements.Add(nested);
    }
}