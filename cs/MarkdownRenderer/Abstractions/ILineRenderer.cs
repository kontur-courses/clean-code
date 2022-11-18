using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions;

public interface ILineRenderer
{
    public string RenderLine(IElement lineElement);
}