using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class HeaderElement : StandardElement,
    IStorageOf<ItalicElement>, IStorageOf<PlainText>, IStorageOf<StrongElement>, IStorageOf<EscapeSequenceElement>
{
    public HeaderElement(string rawContent) : base(rawContent)
    {
    }
}