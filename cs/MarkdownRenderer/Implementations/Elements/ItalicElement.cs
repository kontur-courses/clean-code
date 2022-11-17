using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.Elements;

public class ItalicElement : IElement, IStorageOf<PlainText>
{
    public string RawContent { get; }
    private readonly List<IElement> _nestedElements = new();
    public IEnumerable<IElement> NestedElements => _nestedElements;

    public ItalicElement(string rawContent)
    {
        RawContent = rawContent;
    }

    public void AddNestedElement(IElement nested)
    {
        _nestedElements.Add(nested);
    }
}