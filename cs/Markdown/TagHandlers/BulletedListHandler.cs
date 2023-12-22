using System.Text;

namespace Markdown.TagHandlers
{
    public class BulletedListHandler : IHtmlTagCreator
    {
        private readonly FindTagSettings settings = new(false, true, true);
        private List<int> indexes;
        private readonly List<char> bulletChars = new(){'*', '-', '+'};
        private readonly TagFinder tagFinder = new(new List<IHtmlTagCreator>(){new BoldHandler(), new ItalicHandler()});
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

            var closingIndex = FindIndex(markdownText, openTagIndex);

            var tag = CreateHtmlTag(markdownText, openTagIndex, closingIndex);
            var htmlTag = tag.Text;

            return new Tag(htmlTag, htmlTag.Length);
        }

        private Tag CreateHtmlTag(StringBuilder markdownText, int openTagIndex, int closingIndex)
        {
            markdownText.Remove(closingIndex, 1);

            if (closingIndex == -1)
                markdownText.Append("</ul>");
            else
                markdownText.Insert(closingIndex, "</ul>");

            for (var i = indexes.Count - 1; i >= 0; i--)
            {
                var openSymbol = indexes[i];
                int closingSymbol;

                if (i == indexes.Count - 1)
                    closingSymbol = closingIndex == -1 ? markdownText.Length : closingIndex;
                else
                    closingSymbol = indexes[i + 1] - 1;

                markdownText.Insert(closingSymbol, "</li>");

                if (i == 0 && indexes[i] != 0)
                    markdownText.Remove(indexes[i] - 1, 3);
                else if (i == 0)
                    markdownText.Remove(indexes[i], 2);
                else
                    markdownText.Remove(openSymbol - 1, 3);
                
                markdownText.Insert(openSymbol == 0 ? indexes[i] : openSymbol - 1, "<li>");
            }
            
            markdownText.Insert(indexes[0] == 0 ? openTagIndex : openTagIndex - 1, "<ul>");

            return new Tag(markdownText, closingIndex == -1 ? markdownText.Length : closingIndex);
        }

        private int FindIndex(StringBuilder markdownText, int openTagIndex)
        {
            var result = -1;
            indexes = new List<int> { openTagIndex };

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (markdownText[i] == '\n')
                {
                    if (i + 1 >= markdownText.Length)
                        return i;

                    if (i + 2 < markdownText.Length && bulletChars.Contains(markdownText[i + 1]) &&
                        markdownText[i + 2] == ' ' && IsCorrectOpenTag(markdownText, i + 1))
                        indexes.Add(i + 1);

                    result = i;
                }

                var newTag = tagFinder.FindTag(markdownText, i, settings, "__");
                if (newTag?.Text == null)
                    continue;

                markdownText = newTag.Text;
                i = newTag.Index;

            }
            
            return result;
        }

        private bool IsCorrectOpenTag(StringBuilder markdownText, int openTagIndex) =>
            openTagIndex == 0 || markdownText[openTagIndex - 1] == '\n';
    }
}