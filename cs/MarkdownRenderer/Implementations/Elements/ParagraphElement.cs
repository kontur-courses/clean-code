using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class ParagraphElement : StandardElement,
    IStorageOf<ItalicElement>, IStorageOf<PlainText>, IStorageOf<StrongElement>, 
    IStorageOf<EscapeSequenceElement>, IStorageOf<LinkElement>
{
    public ParagraphElement(string rawContent) : base(rawContent)
    {
    }
}