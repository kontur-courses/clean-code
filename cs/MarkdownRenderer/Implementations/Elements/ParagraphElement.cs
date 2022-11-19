using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class ParagraphElement : StandardElement,
    IContainerFor<PlainText>, IContainerFor<ItalicElement>, IContainerFor<StrongElement>, 
    IContainerFor<LinkElement>, IContainerFor<EscapeSequenceElement>
{
    public ParagraphElement(string rawContent) : base(rawContent)
    {
    }
}