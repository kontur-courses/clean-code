using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class ItalicElement : StandardElement, 
    IStorageOf<PlainText>, IStorageOf<EscapeSequenceElement>, IStorageOf<LinkElement>
{
    public ItalicElement(string rawContent) : base(rawContent)
    {
    }
}