using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlStrongRenderer: HtmlElementRenderer<StrongElement>
{
    protected override string TagText => "strong";
}