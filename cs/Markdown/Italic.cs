using System.Text;

namespace Markdown
{
    public class Italic : IHtmlTagCreator
    {
        private const string ItalicChar = "_";

        public (StringBuilder, int) GetHtmlTag(string markdownText, int openTagIndex)
        {
            var closingTagIndex = FindClosingTagIndex(markdownText, openTagIndex + 1);
            var htmlTag = CreateHtmlTag(markdownText, openTagIndex, closingTagIndex);

            return (htmlTag, closingTagIndex);
        }

        private StringBuilder CreateHtmlTag(string markdownText, int openTagIndex, int closingTagIndex)
        {
            var htmlTag = new StringBuilder(markdownText);

            htmlTag.Remove(closingTagIndex, 1);
            htmlTag.Insert(closingTagIndex, "</em>");
            htmlTag.Remove(openTagIndex, 1);
            htmlTag.Insert(openTagIndex, "<em>");

            return htmlTag;
        }

        private int FindClosingTagIndex(string markdownText, int openTagIndex)
        {
            for (var i = openTagIndex; i < markdownText.Length; i++)
                if (markdownText[i] == '_')
                    return i;

            return -1;
        }
    }
}