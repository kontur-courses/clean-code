using System.Text;

namespace Markdown.Tags
{
    public class HeadingHandler : IHtmlTagCreator
    {
        private FindTagSettings settings = new FindTagSettings(true, true, true);
        public Tag GetHtmlTag(string markdownText, int openTagIndex)
        {
            var correct = IsCorrectOpenTag(markdownText, openTagIndex);

            if (!correct)
                return new Tag(new StringBuilder(markdownText), openTagIndex);

            var closingIndex = FindClosingTagIndex(markdownText, openTagIndex + 1);

            var tag = CreateHtmlTag(markdownText, openTagIndex, closingIndex.index);
            var htmlTag = tag.Text;

            return new Tag(htmlTag, openTagIndex);
        }

        private bool IsCorrectOpenTag(string markdownText, int openTagIndex)
        {
            if (openTagIndex == 0 || markdownText[openTagIndex - 1] == '\n')
                return true;

            return false;
        }

        private (string tag, int index) FindClosingTagIndex(string markdownText, int openTagIndex)
        {
            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (markdownText[i] == '\n')
                    return (markdownText, i);

                var newTag = TagFinder.FindTag(markdownText, i, settings);

                if (newTag == null)
                    continue;

                if (newTag.Index == i)
                    continue;

                markdownText = newTag.Text.ToString();
                i = newTag.Index;
                //markdownText = htmlTag.ToString();

            }

            return (markdownText, -1);
        }

        private Tag CreateHtmlTag(string markdownText, int openTagIndex, int closingIndex)
        {
            var htmlTag = new StringBuilder(markdownText);

            htmlTag.Remove(openTagIndex, 1);
            htmlTag.Insert(openTagIndex, "<h1>");
            htmlTag.Insert(closingIndex == -1 ? htmlTag.Length : closingIndex, "</h1>");

            return new Tag(htmlTag, htmlTag.Length);
        }

        public bool IsHeadingTagSymbol(string markdownText, int i) => markdownText[i] == '#';
    }
}