using System.Text;

namespace Markdown.Tags
{
    public class Bold : IHtmlTagCreator
    {
        public (StringBuilder, int) GetHtmlTag(string markdownText, int openTagIndex)
        {
            var tag = FindClosingTagIndex(markdownText, openTagIndex + 1);
            var htmlTag = tag.Item1;
            var closingTagIndex = tag.Item2;

            if (closingTagIndex == -1)
                return (htmlTag, htmlTag.Length);

            htmlTag = CreateHtmlTag(htmlTag.ToString(), openTagIndex, closingTagIndex);

            return (htmlTag, closingTagIndex + 1);
        }

        private StringBuilder CreateHtmlTag(string markdownText, int openTagIndex, int closingTagIndex)
        {
            var htmlTag = new StringBuilder(markdownText);

            htmlTag.Remove(closingTagIndex, 2);
            htmlTag.Insert(closingTagIndex, "</strong>");
            htmlTag.Remove(openTagIndex, 2);
            htmlTag.Insert(openTagIndex, "<strong>");

            return htmlTag;
        }

        private (StringBuilder, int) FindClosingTagIndex(string markdownText, int openTagIndex)
        {
            var htmlTag = new StringBuilder(markdownText);
            var nestedTadIndex = 0;

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (i + 1 < markdownText.Length &&
                    markdownText[i] == '_' && markdownText[i + 1] == '_')
                    return (htmlTag, i);

                if (markdownText[i] == '_' && markdownText[i - 1] != '_' && markdownText[i + 1] != '_')
                {
                    if (nestedTadIndex == i)
                        continue;

                    var tag = ProcessAnotherTag(markdownText, i);
                    htmlTag = tag.Item1;
                    nestedTadIndex = tag.Item2;

                    markdownText = htmlTag.ToString();
                    i = nestedTadIndex;
                }
            }

            return (htmlTag, -1);
        }

        private (StringBuilder, int) ProcessAnotherTag(string markdownText, int i)
        {
            var italic = new Italic();
            var tag = italic.GetHtmlTag(markdownText, i);
            var htmlText = tag.Item1;
            i = tag.Item2;

            return (htmlText, i);
        }
    }
}