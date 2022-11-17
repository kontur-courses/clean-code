using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlParagraphRenderer : HtmlElementRenderer<ParagraphElement>
{
    protected override string TagText => null!;
    protected override string OpeningTag => "";
    protected override string ClosingTag => "";
}