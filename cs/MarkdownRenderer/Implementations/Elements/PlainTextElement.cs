using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class PlainTextElement : IElement
{
    public string Content { get; }
    string IElement.RawContent => Content;

    IEnumerable<IElement> IElement.NestedElements =>
        throw new InvalidOperationException($"{nameof(PlainTextElement)} cannot contain nested elements.");

    public void AddNestedElement(IElement nested) =>
        throw new InvalidOperationException($"{nameof(PlainTextElement)} cannot contain nested elements.");

    public PlainTextElement(string content)
    {
        Content = content;
    }
}