namespace Markdown
{
    public class Md
    {
        private static readonly string[] ParagraphSeparators = ["\r\n", "\n"];

        private readonly ParagraphRenderer paragraphRenderer = new ();

        public string Render(string markdown)
        {
            var document = new DocumentBuilder();
            foreach (var paragraph in SplitIntoParagraphs(markdown))
            {
                var paragraphHtml = paragraphRenderer.Render(paragraph);
                document.AddParagraph(paragraphHtml, paragraphRenderer.IsListItem(paragraph));
            }

            return document.Build();
        }

        private static IEnumerable<string> SplitIntoParagraphs(string markdown)
        {
            return markdown.Split(ParagraphSeparators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
