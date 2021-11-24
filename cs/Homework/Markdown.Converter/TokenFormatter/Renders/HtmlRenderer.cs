namespace Markdown.TokenFormatter.Renders
{
    public class HtmlRenderer : IRenderer
    {
        public string RenderText(string text) => text;

        public string RenderImage(string src, string alt = "")
            => string.IsNullOrEmpty(alt) ? $"<img src=\"{src}\">" : $"<img src=\"{src}\" alt=\"{alt}\">";
    }
}