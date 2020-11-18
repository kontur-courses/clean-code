namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdown) =>
            new MdHtmlTokenRenderer(markdown).RenderAll(new MdTokenReader(markdown).ReadAll());
    }
}