namespace MarkdownNew
{
    static class MarkdownRender
    {
        public static string Render(string markdown)
        {
            var renderer = new Renderer();
            return renderer.Converter(markdown);
        }
    }
}
