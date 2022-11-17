using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.Elements;

public abstract class StandardElement : IElement
{
    public string RawContent { get; }
    private readonly List<IElement> _nestedElements = new();
    public IEnumerable<IElement> NestedElements => _nestedElements;

    protected StandardElement(string rawContent)
    {
        RawContent = rawContent;
    }

    public bool CanContainNested(Type nestedType)
    {
        return typeof(IElement).IsAssignableFrom(nestedType) &&
               typeof(IStorageOf<>).MakeGenericType(nestedType).IsAssignableFrom(GetType());
    }

    public void AddNestedElement(IElement nested)
    {
        _nestedElements.Add(nested);
    }
}