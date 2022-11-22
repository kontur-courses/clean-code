using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class ParagraphElement : StandardElement
{
    public ParagraphElement(string rawContent) : base(rawContent)
    {
    }
}