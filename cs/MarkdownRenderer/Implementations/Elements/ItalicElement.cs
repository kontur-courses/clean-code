using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class ItalicElement : StandardElement
{
    public ItalicElement(string rawContent) : base(rawContent)
    {
    }
}