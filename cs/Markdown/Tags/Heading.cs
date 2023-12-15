using System.Text;

namespace Markdown.Tags
{
    public class Heading : IHtmlTagCreator
    {
        public (StringBuilder, int) GetHtmlTag(string markdownText, int openTagIndex)
        {
            var tag = CreateHtmlTag(markdownText, openTagIndex);
            var htmlTag = tag.Item1;

            var nestedTag = ProcessAnotherTag(htmlTag.ToString(), openTagIndex + 3);
            htmlTag = nestedTag.Item1;

            return (htmlTag, nestedTag.Item2);
        }

        private (StringBuilder, int) CreateHtmlTag(string markdownText, int openTagIndex)
        {
            var htmlTag = new StringBuilder(markdownText);

            htmlTag.Remove(openTagIndex, 1);
            htmlTag.Insert(openTagIndex, "<h1>");
            htmlTag.Insert(htmlTag.Length, "</h1>");

            return (htmlTag, htmlTag.Length);
        }

        private (StringBuilder, int) ProcessAnotherTag(string markdownText, int index)
        {
            var htmlTag = new StringBuilder(markdownText);
            var closedIndex = 0;

            for (var i = index; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (markdownText[i] == '_' && markdownText[i + 1] == '_')
                {
                    var bold = new Bold();
                    var tag = bold.GetHtmlTag(markdownText, i);
                    htmlTag = tag.Item1;
                    i = tag.Item2;
                    closedIndex = i;
                }
            }

            return (htmlTag, closedIndex);
        }
    }
}