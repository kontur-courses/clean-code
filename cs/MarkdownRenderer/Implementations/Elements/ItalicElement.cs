using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class ItalicElement : StandardElement, 
    IContainerFor<PlainText>, IContainerFor<LinkElement>, IContainerFor<EscapeSequenceElement>
{
    public ItalicElement(string rawContent) : base(rawContent)
    {
    }
}