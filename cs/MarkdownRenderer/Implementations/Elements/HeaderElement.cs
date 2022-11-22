using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class HeaderElement : StandardElement
{
    public HeaderElement(string rawContent) : base(rawContent)
    {
    }
}