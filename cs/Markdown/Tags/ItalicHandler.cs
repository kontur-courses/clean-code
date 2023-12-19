using System.Text;

namespace Markdown.Tags
{
    public class ItalicHandler : IHtmlTagCreator
    {
        public Tag GetHtmlTag(string markdownText, int openTagIndex)
        {
            var closingTagIndex = FindClosingTagIndex(markdownText, openTagIndex + 1);

            if (closingTagIndex == -1)
                return new Tag();

            var htmlTag = CreateHtmlTag(markdownText, openTagIndex, closingTagIndex);

            return new Tag(htmlTag, closingTagIndex + 7);
        }

        internal bool IsItalicTagSymbol(string markdownText, int i)
        {
            if (i > 0)
            {
                return
                    markdownText[i] == '_' && i + 1 >= markdownText.Length ||
                    markdownText[i] == '_' && markdownText[i - 1] != '_' && markdownText[i + 1] != '_';
            }

            return markdownText[i] == '_';
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
                //if (char.IsDigit(markdownText[i]))
                //    return i;

                if (IsItalicTagSymbol(markdownText, i))
                    return i;
            }

            return -1;
        }
    }
}