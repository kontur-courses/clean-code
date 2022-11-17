using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.HtmlRenderers;

public class HtmlItalicRenderer : HtmlElementRenderer<ItalicElement>
{
    protected override string TagText => "em";
}