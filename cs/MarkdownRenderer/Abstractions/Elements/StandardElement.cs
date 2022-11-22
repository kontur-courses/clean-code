using System.Collections.Concurrent;

namespace MarkdownRenderer.Abstractions.Elements;

public abstract class StandardElement : IElement
{
    public string RawContent { get; }
    private readonly List<IElement> _nestedElements = new();
    public IEnumerable<IElement> NestedElements => _nestedElements;

    private static readonly ConcurrentDictionary<Type, IReadOnlySet<Type>> CachedValidNestedTypes = new();

    protected StandardElement(string rawContent)
    {
        RawContent = rawContent;
    }

    public void AddNestedElement(IElement nested) => _nestedElements.Add(nested);
}