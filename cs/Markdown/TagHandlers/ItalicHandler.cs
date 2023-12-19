using System.Text;

namespace Markdown.TagHandlers
{
    public class ItalicHandler : IHtmlTagCreator
    {
        public Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex)
        {
            var closingTagIndex = FindClosingTagIndex(markdownText, openTagIndex + 1);

            if (closingTagIndex == -1)
                return new Tag();

            var htmlTag = CreateHtmlTag(markdownText, openTagIndex, closingTagIndex);

            return new Tag(htmlTag, closingTagIndex + 7);
        }

        internal bool IsItalicTagSymbol(StringBuilder markdownText, int i)
        {
            if (i > 0)
                return
                    (markdownText[i] == '_' && i + 1 >= markdownText.Length) ||
                    (markdownText[i] == '_' && markdownText[i - 1] != '_' && markdownText[i + 1] != '_');

            return markdownText[i] == '_';
        }

        private StringBuilder? CreateHtmlTag(StringBuilder markdownText, int openTagIndex, int closingTagIndex)
        {
            if (openTagIndex + 1 == closingTagIndex)
                return markdownText;

            markdownText.Remove(closingTagIndex, 1);
            markdownText.Insert(closingTagIndex, "</em>");
            markdownText.Remove(openTagIndex, 1);
            markdownText.Insert(openTagIndex, "<em>");

            return markdownText;
        }

        private int FindClosingTagIndex(StringBuilder markdownText, int openTagIndex)
        {
            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (char.IsDigit(markdownText[i]))
                    return i;

                if (IsItalicTagSymbol(markdownText, i))
                    return i;
            }

            return -1;
        }
    }
}