using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Converter
    {
        private static StringBuilder _htmlText = new StringBuilder();

        public static string ConvertToHtml(string[] markdownParagraphs, List<List<Tag>> allTags)
        {
            for (var position = 0; position < markdownParagraphs.Length; position++)
            {
                AddHtmlParagraph(markdownParagraphs[position], allTags[position]);
                if (position < markdownParagraphs.Length - 1)
                    _htmlText.Append("\r\n");
            }

            var result = _htmlText.ToString();
            _htmlText.Clear();
            return result;
        }

        private static void AddHtmlParagraph(string paragraph, List<Tag> paragraphTags)
        {
            var currentPosition = 0;
            foreach (var currentTag in paragraphTags)
            {
                if (currentPosition < currentTag.Position)
                    _htmlText.Append(paragraph.Substring(currentPosition,
                        currentTag.Position - currentPosition));
                _htmlText.Append(Comparison.GetHtmlSymbol(currentTag));
                currentPosition = currentTag.Position + currentTag.Length;
            }

            if (currentPosition < paragraph.Length)
                _htmlText.Append(paragraph.Substring(currentPosition, paragraph.Length - currentPosition));
        }
    }
}