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
        lock (CachedValidNestedTypes)
        {
            if (!CachedValidNestedTypes.ContainsKey(thisType))
                CachedValidNestedTypes[thisType] = thisType.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsGenericType)
                    .Where(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IStorageOf<>))
                    .Select(interfaceType => interfaceType.GetGenericArguments().First())
                    .ToHashSet();
        }
    }

    public bool CanContainNested(Type nestedType)
    {
        lock (CachedValidNestedTypes)
        {
            return CachedValidNestedTypes[GetType()].Contains(nestedType);
        }
    }

    public void AddNestedElement(IElement nested)
    {
        if (!CanContainNested(nested.GetType()))
            throw new ArgumentException($"{GetType().Name} cannot contain element {nested.GetType().Name}");
        _nestedElements.Add(nested);
    }
}