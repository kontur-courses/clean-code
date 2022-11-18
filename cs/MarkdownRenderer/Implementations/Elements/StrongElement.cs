using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class StrongElement : StandardElement,
    IStorageOf<ItalicElement>, IStorageOf<PlainText>, IStorageOf<EscapeSequenceElement>, IStorageOf<LinkElement>
{
    public StrongElement(string rawContent) : base(rawContent)
    {
    }
}