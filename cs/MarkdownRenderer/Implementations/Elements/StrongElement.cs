using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class StrongElement : StandardElement
{
    public StrongElement(string rawContent) : base(rawContent)
    {
    }
}