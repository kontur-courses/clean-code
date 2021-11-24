using System.Text;

namespace Markdown.Render
{
    public class HtmlRender
    {
        private readonly IRenderer[] renderers;
        private readonly IRenderer startRenderer;
        
        public HtmlRender()
        {
            renderers = new IRenderer[]
            {
                new Renderer(TextType.Header, "<h1>", "</h1>\n"),
                new Renderer(TextType.Paragraph, "<p>", "</p>\n"),
                new Renderer(TextType.BoldText, "<strong>", "</strong>"),
                new Renderer(TextType.ItalicText, "<em>", "</em>"),
                new Renderer(TextType.PlainText, "", "")
            };
            startRenderer = new Renderer(TextType.Body, "", "");
        }

        public string Render(HyperTextElement textGraph)
        {
            var render = new StringBuilder();
            startRenderer.Render(textGraph, renderers, render);
            return render.ToString().TrimEnd('\n');
        }
    }
}