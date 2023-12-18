using System.Text;

namespace Markdown.Tags
{
    public class Italic : IHtmlTagCreator
    {
        public (StringBuilder, int) GetHtmlTag(string markdownText, int openTagIndex)
        {
            var closingTagIndex = FindClosingTagIndex(markdownText, openTagIndex + 1);
            var htmlTag = CreateHtmlTag(markdownText, openTagIndex, closingTagIndex);

            return (htmlTag, closingTagIndex);
        }

        private StringBuilder CreateHtmlTag(string markdownText, int openTagIndex, int closingTagIndex)
        {
            var htmlTag = new StringBuilder(markdownText);
            if (openTagIndex + 1 == closingTagIndex)
                return htmlTag;

            htmlTag.Remove(closingTagIndex, 1);
            htmlTag.Insert(closingTagIndex, "</em>");
            htmlTag.Remove(openTagIndex, 1);
            htmlTag.Insert(openTagIndex, "<em>");

            return htmlTag;
        }

        private int FindClosingTagIndex(string markdownText, int openTagIndex)
        {
            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (char.IsDigit(markdownText[i]))
                    return i;

                if (markdownText[i] == '_' && i + 1 >= markdownText.Length || markdownText[i - 1] < 0)
                    return i;
                
                if (markdownText[i] == '_' && markdownText[i - 1] != '_' && markdownText[i + 1] != '_')
                    return i;
            }

            return -1;
        }
    }
}