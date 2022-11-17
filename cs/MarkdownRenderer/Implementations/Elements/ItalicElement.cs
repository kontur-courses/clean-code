using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.Elements;

public class ItalicElement : StandardElement, IStorageOf<PlainText>
{
    public ItalicElement(string rawContent) : base(rawContent)
    {
    }
}