namespace Markdown
{
    public static class Md
    {
        private static readonly string[] ParagraphSeparators = ["\r\n", "\n"];

        public static string Render(string markdown)
        {
            var paragraphs = markdown.Split(ParagraphSeparators, StringSplitOptions.None);
            return HtmlParagraphRenderer.Render(paragraphs);
        }
    }
}
