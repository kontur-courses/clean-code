namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdown) =>
            new MdTokenHtmlRenderer(markdown).RenderAll(new MdTokenReader(markdown).ReadAll());
    }
}