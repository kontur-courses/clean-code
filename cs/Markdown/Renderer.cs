using System;

namespace Markdown
{
    public static class Renderer
    {
        public static string Render(string textInMarkdown)
        {
            if (string.IsNullOrEmpty(textInMarkdown))
                throw new ArgumentException("String is null or empty");
            var paragraphs = textInMarkdown.Split("\r\n");
            var analyzer = new Analyzer();
            var tagsForAllParagraphs = analyzer.GetTagsForAllParagraphs(paragraphs);
            if (tagsForAllParagraphs.Count == 0)
                return textInMarkdown;
            var htmlText = Converter.ConvertToHtml(paragraphs, tagsForAllParagraphs);
            return htmlText;
        }
    }
}