using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class EscapeSequenceElement : StandardElement,
    IContainerFor<PlainText>
{
    public EscapeSequenceElement(string rawContent) : base(rawContent)
    {
    }
}