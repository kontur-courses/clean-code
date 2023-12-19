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
            var htmlTag = newTag.Text;
            var closingTagIndex = newTag.Index;

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

        private Tag FindClosingTagIndex(StringBuilder markdownText, int openTagIndex)
        {
            var resultTag = new Tag(markdownText, -1);

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (IsBoldTagSymbol(markdownText, i))
                {
                    resultTag.Index = i;
                    return resultTag;
                }

                var newTag = TagFinder.FindTag(markdownText, i, settings);

                if (newTag == null || newTag.Text == null)
                    continue;

                resultTag.NestedTags.Add(newTag);

                if (newTag.Index == i)
                    continue;

                markdownText = newTag.Text;
                i = newTag.Index;
                resultTag.Index = i;
            }

            return resultTag;
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