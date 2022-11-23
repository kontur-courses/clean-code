namespace MarkdownRenderer.Abstractions.Elements;

public interface IElement
{
    IEnumerable<IElement> NestedElements { get; }
    void AddNested(IElement nested);
}