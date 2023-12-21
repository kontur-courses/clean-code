using System.Text;

namespace Markdown.TagHandlers
{
    public class BulletedListHandler : IHtmlTagCreator
    {
        private FindTagSettings settings = new(false, false, false);
        private List<int> indexes;
        private List<char> bulletChars = new List<char>(){'*', '-', '+'};
        public bool IsTagSymbol(StringBuilder markdownText, int currentIndex) =>
            bulletChars.Contains(markdownText[currentIndex]) && currentIndex + 1 < markdownText.Length && markdownText[currentIndex + 1] == ' ';

        public Tag FindTag(StringBuilder markdownText, int currentIndex, FindTagSettings settings, string? closingTagParent)
        {
            if (settings.SearchForHeading && IsTagSymbol(markdownText, currentIndex))
            {
                var screeningSymbolsCount = TagFindHelper.ScreeningCheck(markdownText, currentIndex);

                if (screeningSymbolsCount == 1)
                    return new Tag(markdownText.Remove(currentIndex - 1, 1), currentIndex);
                if (screeningSymbolsCount == 2)
                    currentIndex -= 2;
            }

            return GetHtmlTag(markdownText, currentIndex, closingTagParent);
        }

        private Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex, string? closingTagParent)
        {
            var correct = IsCorrectOpenTag(markdownText, openTagIndex);

            if (!correct)
                return new Tag(markdownText, openTagIndex);

            var closingIndex = FindIndexes(markdownText, openTagIndex);

            var tag = CreateHtmlTag(markdownText, openTagIndex, closingIndex);
            var htmlTag = tag.Text;

            return new Tag(htmlTag, tag.Index);
        }

        private Tag CreateHtmlTag(StringBuilder markdownText, int openTagIndex, int closingIndex)
        {
            markdownText.Insert(closingIndex == -1 ? markdownText.Length : closingIndex, "\n</ul>");

            for (var i = indexes.Count - 1; i >= 0; i--)
            {
                var openSymbol = indexes[i];
                var closingSymbol = 0;

                if (i == indexes.Count - 1)
                    closingSymbol = closingIndex == -1 ? markdownText.Length - 6 : closingIndex;
                else
                    closingSymbol = indexes[i + 1] - 1;

                markdownText.Insert(closingSymbol, "</li>");
                markdownText.Remove(openSymbol, 2);
                markdownText.Insert(openSymbol, "<li>");
            }

            markdownText.Insert(openTagIndex, "<ul>\n");

            return new Tag(markdownText, closingIndex == -1 ? markdownText.Length : closingIndex);
        }

        private int FindIndexes(StringBuilder markdownText, int openTagIndex)
        {
            var result = -1;
            indexes = new List<int>()
            {
                openTagIndex
            };

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (markdownText[i] == '\n')
                {
                    if (IsCorrectOpenTag(markdownText, i + 1))
                        indexes.Add(i + 1);
                    i += 2;
                    result = markdownText.Length;
                }
            }

            return result;
        }

        private bool IsCorrectOpenTag(StringBuilder markdownText, int openTagIndex) =>
            openTagIndex == 0 || markdownText[openTagIndex - 1] == '\n';
    }
}