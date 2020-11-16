using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Md
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

        private static IEnumerable<string> GetParagraphs(string text)
        {
            var startIndex = 0;
            var index = text.IndexOf("\n\n", StringComparison.Ordinal);
            while (index != -1)
            {
                yield return text.Substring(startIndex, index - startIndex);
                startIndex = index;
                index = text.IndexOf("\n\n", startIndex + 1, StringComparison.Ordinal);
            }

            yield return text.Substring(startIndex);
        }

        private static string RenderParagraph(string paragraph)
        {
            var tags = Tags.BuildTags(paragraph);

            return MarkdownToHtmlConverter.Convert(paragraph, tags.PairTags, tags.SingleTags);
        }
    }
}