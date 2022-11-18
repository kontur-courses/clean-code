namespace MarkdownRenderer.Abstractions.Elements;

public interface IElement
{
    string RawContent { get; }
    IEnumerable<IElement> NestedElements { get; }

    public bool CanContainNested(Type nestedType);

    void AddNestedElement(IElement nested);
}