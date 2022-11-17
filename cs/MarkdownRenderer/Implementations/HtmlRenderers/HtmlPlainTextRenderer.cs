using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlPlainTextRenderer : HtmlElementRenderer<PlainText>
{
    protected override string TagText => null!;

    public override string Render(PlainText elem, IReadOnlyDictionary<Type, IElementRenderer> renderers) => 
        elem.Content;
}