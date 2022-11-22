using MarkdownRenderer.Abstractions.ElementsRenderers;
using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlPlainTextRenderer : HtmlElementRenderer<PlainTextElement>
{
    protected override string TagText => null!;

    public override string Render(PlainTextElement elem, IRenderersProvider renderersProvider) => 
        elem.Content;
}