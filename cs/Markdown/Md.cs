using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string markdown)
        {
            var renderedMarkdown = new StringBuilder();
            foreach (var paragraph in GetParagraphs(markdown))
            {
                renderedMarkdown.Append(RenderParagraph(paragraph));
            }

            return renderedMarkdown.ToString();
        }

        private static string RenderParagraph(string paragraph)
        {
            var tags = GetTags(paragraph);
            var textWithouttTags = DeleteTags(paragraph, tags);
            return InsertHtmlTags(textWithouttTags, tags);
        }

        private static Tag[] GetTags(string text)
        {
            throw new System.NotImplementedException();
        }

        private static string DeleteTags(string text, Tag[] tags)
        {
            throw new System.NotImplementedException();
        }

        private static string[] GetParagraphs(string text)
        {
            throw new System.NotImplementedException();
        }

        private static string InsertHtmlTags(string text, Tag[] tags)
        {
            throw new System.NotImplementedException();
        }
    }
}