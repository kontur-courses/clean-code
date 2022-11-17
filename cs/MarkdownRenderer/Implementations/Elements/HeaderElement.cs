using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.Elements;

public class HeaderElement : StandardElement,
    IStorageOf<ItalicElement>, IStorageOf<PlainText>, IStorageOf<StrongElement>
{
    public HeaderElement(string rawContent) : base(rawContent)
    {
    }
}