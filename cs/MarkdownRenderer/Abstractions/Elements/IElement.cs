namespace MarkdownRenderer.Abstractions.Elements;

public interface IElement
{
    string RawContent { get; }
    IEnumerable<IElement> NestedElements { get; }
    void AddNestedElement(IElement nested);
}