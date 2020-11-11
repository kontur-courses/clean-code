using System.Text;
using MarkdownParser.Concrete.Bold;
using Rendering.Html.Abstract;

namespace Rendering.Html.Impl
{
    public class BoldElementRenderer : MarkdownElementRenderer<MarkdownElementBold>, IHtmlRendererDependent
    {
        private HtmlRenderer htmlRenderer;

        public override string TagText { get; } = "strong";

        public override string RenderElement(MarkdownElementBold element)
        {
            var content = new StringBuilder();
            content.Append(this.OpenTag());

            foreach (var markdownElement in element.Content)
            {
                var renderedElement = htmlRenderer.Render(markdownElement);
                content.Append(renderedElement);
            }

            content.Append(this.CloseTag());
            return content.ToString();
        }

        public void SetRenderer(HtmlRenderer renderer) => htmlRenderer = renderer;
    }
}