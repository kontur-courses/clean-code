using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.Elements;

public class StrongElement : StandardElement, IStorageOf<ItalicElement>, IStorageOf<PlainText>
{
    public StrongElement(string rawContent) : base(rawContent)
    {
    }
}