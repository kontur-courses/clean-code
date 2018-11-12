namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdown)
        {
            IMdParser parser = new MdParser();
            IRenderer renderer = new HtmlRenderer();
            var result = parser.Parse(markdown);

            return renderer.Render(result);
        }
    }
}