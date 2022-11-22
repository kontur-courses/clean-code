using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class EscapeSequenceElement : StandardElement
{
    public EscapeSequenceElement(string rawContent) : base(rawContent)
    {
    }
}