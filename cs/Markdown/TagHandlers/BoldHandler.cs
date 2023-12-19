using System.Text;

namespace Markdown.TagHandlers
{
    public class BoldHandler : IHtmlTagCreator
    {
        private readonly FindTagSettings settings = new(false, false, true);
        private const char Bold = '_';
        private const int MdTagLength = 4;
        private const int HtmlTagLength = 17;

        internal bool IsBoldTagSymbol(StringBuilder markdownText, int i) =>
            i + 1 < markdownText.Length && markdownText[i] == Bold && markdownText[i + 1] == Bold;

        public Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex)
        {
            var newTag = FindClosingTagIndex(markdownText, openTagIndex + 2);
            var htmlTag = newTag.Text;
            var closingTagIndex = newTag.Index;

            if (closingTagIndex == -1)
                return new Tag(htmlTag, htmlTag!.Length);

            htmlTag = CreateHtmlTag(htmlTag, openTagIndex, closingTagIndex);
            var newHtmlTagLength = closingTagIndex + (HtmlTagLength - MdTagLength);

            return new Tag(htmlTag, newHtmlTagLength);
        }

        private StringBuilder? CreateHtmlTag(StringBuilder? markdownText, int openTagIndex, int closingTagIndex)
        {
            markdownText!.Remove(closingTagIndex, 2);
            markdownText.Insert(closingTagIndex, "</strong>");
            markdownText.Remove(openTagIndex, 2);
            markdownText.Insert(openTagIndex, "<strong>");

            return markdownText;
        }

        private Tag FindClosingTagIndex(StringBuilder markdownText, int openTagIndex)
        {
            var resultTag = new Tag(markdownText, -1);
            var correctPartOfWord = true;
            var closingTagFound = false;

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (markdownText[i] == ' ' && markdownText[i - 1] != '>')
                    correctPartOfWord = false;

                if (char.IsDigit(markdownText[i]))
                {
                    resultTag.Index = -1;
                    return resultTag;
                }

                if (IsBoldTagSymbol(markdownText, i) && TagSettings.IsCorrectClosingSymbol(markdownText, i, Bold))
                {
                    closingTagFound = true;

                    if (!correctPartOfWord && TagSettings.IsHalfOfWord(markdownText, i))
                    {
                        resultTag.Index = -1;
                        return resultTag;
                    }

                    resultTag.Index = i;
                    return resultTag;
                }

                var newTag = TagFinder.FindTag(markdownText, i, settings);
                if (newTag?.Text == null)
                    continue;

                resultTag.NestedTags.Add(newTag);

                //if (!closingTagFound)
                //    return resultTag;

                //  markdownText = newTag.Text;
                i = newTag.Index;
                correctPartOfWord = true;
            }

            return resultTag;
        }
    }
}