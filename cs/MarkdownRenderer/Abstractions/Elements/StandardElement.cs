namespace MarkdownRenderer.Abstractions.Elements;

public abstract class StandardElement : IElement
{
    private readonly List<IElement> _nestedElements = new();
    public IEnumerable<IElement> NestedElements => _nestedElements;
    public void AddNested(IElement nested) => _nestedElements.Add(nested);
}