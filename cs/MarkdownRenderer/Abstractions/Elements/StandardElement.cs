namespace MarkdownRenderer.Abstractions.Elements;

public abstract class StandardElement : IElement
{
    public string RawContent { get; }
    private readonly List<IElement> _nestedElements = new();
    public IEnumerable<IElement> NestedElements => _nestedElements;

    private static readonly Dictionary<Type, IReadOnlySet<Type>> CachedValidNestedTypes = new();

    protected StandardElement(string rawContent)
    {
        RawContent = rawContent;

        var thisType = GetType();
        if (!CachedValidNestedTypes.ContainsKey(thisType))
            CachedValidNestedTypes[thisType] = thisType.GetInterfaces()
                .Where(interfaceType => interfaceType.IsGenericType)
                .Where(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IStorageOf<>))
                .Select(interfaceType => interfaceType.GetGenericArguments().First())
                .ToHashSet();
    }

    public bool CanContainNested(Type nestedType) =>
        CachedValidNestedTypes[GetType()].Contains(nestedType);

    public void AddNestedElement(IElement nested) =>
        _nestedElements.Add(nested);
}