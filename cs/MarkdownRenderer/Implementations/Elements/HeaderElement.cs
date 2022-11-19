using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class HeaderElement : StandardElement,
    IContainerFor<PlainText>, IContainerFor<ItalicElement>, IContainerFor<StrongElement>,
    IContainerFor<LinkElement>, IContainerFor<EscapeSequenceElement>
{
    public HeaderElement(string rawContent) : base(rawContent)
    {
    }
}