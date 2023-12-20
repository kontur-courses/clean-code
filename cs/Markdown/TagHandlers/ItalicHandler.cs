using System.Text;

namespace Markdown.TagHandlers
{
    public class ItalicHandler : IHtmlTagCreator
    {
        private const char Italic = '_';
        private const int MdTagLength = 2;
        private const int HtmlTagLength = 9;

        public bool IsTagSymbol(StringBuilder markdownText, int i)
        {
            if (i > 0)
                return
                    (markdownText[i] == Italic && i + 1 >= markdownText.Length) ||
                    (markdownText[i] == Italic && markdownText[i - 1] != Italic && markdownText[i + 1] != Italic);

            return markdownText[i] == Italic;
        }

        public Tag FindTag(StringBuilder markdownText, int currentIndex, FindTagSettings settings, string? closingTagParent)
        {
            var screeningSymbolsCount = TagFindHelper.ScreeningCheck(markdownText, currentIndex);

            if (screeningSymbolsCount == 1)
                return new Tag(markdownText.Remove(currentIndex - 1, 1), currentIndex);
            if (screeningSymbolsCount == 2)
                currentIndex -= 2;

            if (!TagFindHelper.IsCorrectOpenSymbol(markdownText, currentIndex))
                return null;

            return GetHtmlTag(markdownText, currentIndex, closingTagParent);
        }

        public Tag? GetHtmlTag(StringBuilder markdownText, int openTagIndex, string? parentClosingTag)
        {
            var closingTagIndex = FindClosingTagIndex(markdownText, openTagIndex + 1, parentClosingTag);

            if (closingTagIndex == -1)
                return null;

            var htmlTag = CreateHtmlTag(markdownText, openTagIndex, closingTagIndex);
            var newHtmlTagLength = closingTagIndex + (HtmlTagLength - MdTagLength);

            return new Tag(htmlTag, newHtmlTagLength);
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

        private int FindClosingTagIndex(StringBuilder markdownText, int openTagIndex, string parentClosingTag)
        {
            var oneWord = true;
            BoldHandler bold = new BoldHandler();

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (markdownText[i] == ' ')
                    oneWord = false;

                if (char.IsDigit(markdownText[i]))
                    return i;

                if (IsTagSymbol(markdownText, i) && TagFindHelper.IsCorrectClosingSymbol(markdownText, i, Italic))
                {
                    if (!oneWord && TagFindHelper.IsHalfOfWord(markdownText, i))
                        return -1;

                    return i;
                }

                if (bold.IsTagSymbol(markdownText, i))
                    if (parentClosingTag == "__")
                        return -1;
                    else
                    {
                        var newTag = bold.GetHtmlTag(markdownText, i, "_");
                        i = newTag.Index;
                    }
            }

            return -1;
        }
    }
}