namespace Markdown
{
    public static class Md
    {
        private static readonly string[] ParagraphSeparators = ["\r\n", "\n"];

        public static string Render(string markdown)
        {
            var document = new DocumentBuilder();
            foreach (var paragraph in SplitIntoParagraphs(markdown))
            {
                var paragraphHtml = ParagraphRenderer.Render(paragraph);
                if (ParagraphRenderer.IsListItem(paragraph))
                    document.AddListItem(paragraphHtml);
                else
                    document.AddParagraph(paragraphHtml);
            }

            return document.Build();
        }

        private static string[] SplitIntoParagraphs(string markdown)
        {
            return markdown.Split(ParagraphSeparators, StringSplitOptions.None);
        }
    }
}
