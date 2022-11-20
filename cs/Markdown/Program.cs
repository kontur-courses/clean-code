using Markdown.Html;

namespace Markdown
{
    class Program
    {
        static void Main()
        {
            var markdownText = "";
            var htmlText = Renderer.Render<HtmlTokenParser>(markdownText);
        }
    }
}