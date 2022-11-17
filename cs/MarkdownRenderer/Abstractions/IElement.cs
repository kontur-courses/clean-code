namespace MarkdownRenderer.Abstractions;

public interface IElement
{
    string RawContent { get; }
    IEnumerable<IElement> NestedElements { get; }

    public bool CanContainNested(Type nestedType)
    {
        return typeof(IStorageOf<>).MakeGenericType(nestedType).IsAssignableFrom(GetType());
    }

    void AddNestedElement(IElement nested);
}