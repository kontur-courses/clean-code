using System.Text;
using MarkdownParser.Concrete.Header;
using Rendering.Html.Abstract;

namespace Rendering.Html.Impl
{
    public class HeaderElementRenderer : MarkdownElementRenderer<MarkdownElementHeader>, IHtmlRendererDependent
    {
        private HtmlRenderer htmlRenderer;

        public override string TagText { get; } = "h1";
        public override string RenderElement(MarkdownElementHeader element)
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

        public void SetRenderer(HtmlRenderer renderer) => this.htmlRenderer = renderer;
    }
}