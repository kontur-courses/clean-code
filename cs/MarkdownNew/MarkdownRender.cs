namespace Markdown
{
    static class MarkdownRender
    {
        public static string Render(string markdown)
        {
            var renderer = new Renderer(TagsPairsDictionary.GetTagsPairs());
            return renderer.Convert(markdown) as string;
        }
    }
}
