using MarkdownRenderer.Abstractions.ElementsRenderers;
using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlPlainTextRenderer : HtmlElementRenderer<PlainText>
{
    protected override string TagText => null!;

    public override string Render(PlainText elem, IRenderersProvider renderersProvider) => 
        elem.Content;
}