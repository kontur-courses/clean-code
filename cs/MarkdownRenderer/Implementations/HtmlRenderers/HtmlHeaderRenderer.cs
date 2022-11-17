using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlHeaderRenderer : HtmlElementRenderer<HeaderElement>
{
    protected override string TagText => "h1";
}