using System.Text;

namespace Markdown.TagHandlers
{
    public class HeadingHandler : IHtmlTagCreator
    {
        private FindTagSettings settings = new FindTagSettings(true, true, true);
        public Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex)
        {
            var correct = IsCorrectOpenTag(markdownText, openTagIndex);

            if (!correct)
                return new Tag(markdownText, openTagIndex);

            var closingIndex = FindClosingTagIndex(markdownText, openTagIndex + 1);

            var tag = CreateHtmlTag(markdownText, openTagIndex, closingIndex.Index);
            var htmlTag = tag.Text;

            return new Tag(htmlTag, tag.Index);
        }

        private bool IsCorrectOpenTag(StringBuilder markdownText, int openTagIndex)
        {
            if (openTagIndex == 0 || markdownText[openTagIndex - 1] == '\n')
                return true;

            return false;
        }

        private Tag FindClosingTagIndex(StringBuilder markdownText, int openTagIndex)
        {
            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (markdownText[i] == '\n')
                    return new Tag(markdownText, i - 1);

                var newTag = TagFinder.FindTag(markdownText, i, settings);

                if (newTag == null || newTag!.Text == null)
                    continue;

                if (newTag.Index == i)
                    continue;

                markdownText = newTag.Text;
                i = newTag.Index;
                //markdownText = htmlTag.ToString();

            }

            return new(markdownText, -1);
        }

        private Tag CreateHtmlTag(StringBuilder markdownText, int openTagIndex, int closingIndex)
        {
            //  var htmlTag = new StringBuilder(markdownText);
            markdownText.Insert(closingIndex == -1 ? markdownText.Length : closingIndex, "</h1>");
            markdownText.Remove(openTagIndex, 1);
            markdownText.Insert(openTagIndex, "<h1>");

            return new Tag(markdownText, closingIndex == -1 ? markdownText.Length : closingIndex);
        }

        public bool IsHeadingTagSymbol(StringBuilder markdownText, int i) => markdownText[i] == '#';
    }
}