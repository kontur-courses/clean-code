using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class PlainText : IElement
{
    public string Content { get; }
    string IElement.RawContent => Content;

    IEnumerable<IElement> IElement.NestedElements =>
        throw new InvalidOperationException($"{nameof(PlainText)} cannot contain nested elements.");

    public bool CanContainNested(Type nestedType) => false;

    public void AddNestedElement(IElement nested) =>
        throw new InvalidOperationException($"{nameof(PlainText)} cannot contain nested elements.");

    public PlainText(string content)
    {
        Content = content;
    }
}