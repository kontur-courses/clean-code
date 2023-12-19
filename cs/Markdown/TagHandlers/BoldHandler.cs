using System.Text;

namespace Markdown.TagHandlers
{
    public class BoldHandler : IHtmlTagCreator
    {
        private Md md = new();
        private FindTagSettings settings = new FindTagSettings(false, false, true);

        public Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex)
        {
            var newTag = FindClosingTagIndex(markdownText, openTagIndex + 2);
            var htmlTag = newTag.tag;
            var closingTagIndex = newTag.index;

            if (closingTagIndex == -1)
                return new Tag(htmlTag, htmlTag!.Length);

            htmlTag = CreateHtmlTag(htmlTag, openTagIndex, closingTagIndex);

            return new Tag(htmlTag, closingTagIndex + 14);
        }

        internal bool IsBoldTagSymbol(StringBuilder markdownText, int i) =>
            i + 1 < markdownText.Length && markdownText[i] == '_' && markdownText[i + 1] == '_';

        private StringBuilder? CreateHtmlTag(StringBuilder? markdownText, int openTagIndex, int closingTagIndex)
        {
            markdownText!.Remove(closingTagIndex, 2);
            markdownText.Insert(closingTagIndex, "</strong>");
            markdownText.Remove(openTagIndex, 2);
            markdownText.Insert(openTagIndex, "<strong>");

            return markdownText;
        }

        private (StringBuilder? tag, int index) FindClosingTagIndex(StringBuilder markdownText, int openTagIndex)
        {
            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (IsBoldTagSymbol(markdownText, i))
                    return (markdownText, i);

                var newTag = TagFinder.FindTag(markdownText, i, settings);

                if (newTag == null || newTag.Text == null)
                    continue;

                if (newTag.Index == i)
                    continue;

                markdownText = newTag.Text;
                i = newTag.Index;
            }

            return (markdownText, -1);
        }

        private (StringBuilder, int) ProcessAnotherTag(StringBuilder markdownText, int i)
        {
            var italic = new ItalicHandler();
            var tag = italic.GetHtmlTag(markdownText, i);
            var htmlText = tag.Text;
            i = tag.Index;

            return (htmlText, i)!;
        }
    }
}