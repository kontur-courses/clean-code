using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class StrongElement : StandardElement,
    IContainerFor<PlainText>, IContainerFor<ItalicElement>, 
    IContainerFor<LinkElement>, IContainerFor<EscapeSequenceElement>
{
    public StrongElement(string rawContent) : base(rawContent)
    {
    }
}