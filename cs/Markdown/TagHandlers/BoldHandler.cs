using System.Text;

namespace Markdown.TagHandlers
{
    public class BoldHandler : IHtmlTagCreator
    {
        private readonly FindTagSettings settings = new(false, false, true);
        private const char Bold = '_';
        private const int MdTagLength = 4;
        private const int HtmlTagLength = 17;

        public bool IsTagSymbol(StringBuilder markdownText, int i) =>
            i + 1 < markdownText.Length && markdownText[i] == Bold && markdownText[i + 1] == Bold;

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

        public Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex, string? parentClosingTag)
        {
            var newTag = FindClosingTagIndex(markdownText, openTagIndex + 2, parentClosingTag);
            var htmlTag = newTag.Text;
            var closingTagIndex = newTag.Index;

            if (closingTagIndex == -1)
                return new Tag(htmlTag, htmlTag!.Length);

            if (parentClosingTag == "_")
                return new Tag(htmlTag, closingTagIndex);

            htmlTag = CreateHtmlTag(htmlTag, openTagIndex, closingTagIndex);
            var newHtmlTagLength = closingTagIndex + (HtmlTagLength - MdTagLength);

            return new Tag(htmlTag, newHtmlTagLength);
        }

        private StringBuilder CreateHtmlTag(StringBuilder markdownText, int openTagIndex, int closingTagIndex)
        {
            markdownText.Remove(closingTagIndex, 2);
            markdownText.Insert(closingTagIndex, "</strong>");
            markdownText.Remove(openTagIndex, 2);
            markdownText.Insert(openTagIndex, "<strong>");

            return markdownText;
        }

        public Tag FindClosingTagIndex(StringBuilder markdownText, int openTagIndex, string closParTag)
        {
            TagFinder tagFinder = new(new List<IHtmlTagCreator>
            {
                new HeadingHandler(), new BoldHandler(), new ItalicHandler()
            });

            var resultTag = new Tag(markdownText, -1);
            var correctPartOfWord = true;
            var resultTagText = markdownText.ToString();
            var haveLetters = false;

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (markdownText[i] == ' ' && markdownText[i - 1] != '>')
                    correctPartOfWord = false;

                if (char.IsLetter(markdownText[i]))
                    haveLetters = true;

                if (IsTagSymbol(markdownText, i) && TagFindHelper.IsCorrectClosingSymbol(markdownText, i, Bold))
                {
                    if (!correctPartOfWord && TagFindHelper.IsHalfOfWord(markdownText, i))
                    {
                        resultTag.Index = -1;
                        if (closParTag == "_")
                            resultTag.Text = new StringBuilder(resultTagText.Substring(0, i));
                        else
                            resultTag.Text = markdownText;
                        return resultTag;
                    }

                    if (haveLetters)
                    {
                        resultTag.Index = i;
                        return resultTag;
                    }

                    return resultTag;
                }

                var newTag = tagFinder.FindTag(markdownText, i, settings, "__");
                if (newTag?.Text == null)
                    continue;

                resultTag.NestedTags.Add(newTag);
                markdownText = newTag.Text;
                i = newTag.Index;

                correctPartOfWord = true;
            }

            return resultTag;
        }
    }
}