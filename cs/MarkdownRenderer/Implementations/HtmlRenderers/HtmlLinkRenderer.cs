using MarkdownRenderer.Abstractions.ElementsRenderers;
using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlLinkRenderer : HtmlElementRenderer<LinkElement>
{
    protected override string TagText => "a";

    public override string Render(LinkElement elem, IRenderersProvider renderersProvider)
    {
        return $"<a href=\"{elem.Destination}\">{elem.Title}</a>";
    }
}