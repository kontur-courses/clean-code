namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            return MarkdownToHtmlConverter.Convert(text, TagParser.GetTags(text));
        }
    }
}