using System.Text;

namespace Markdown.Tags
{
    public class BoldHandler : IHtmlTagCreator
    {
        private Md md = new();
        private FindTagSettings settings = new FindTagSettings(false, false, true);

        public Tag GetHtmlTag(string markdownText, int openTagIndex)
        {
            var newTag = FindClosingTagIndex(markdownText, openTagIndex + 2);
            var htmlTag = newTag.tag;
            var closingTagIndex = newTag.index;

            if (closingTagIndex == -1)
                return new Tag(htmlTag, htmlTag.Length);

            htmlTag = CreateHtmlTag(htmlTag.ToString(), openTagIndex, closingTagIndex);

            return new Tag(htmlTag, closingTagIndex + 14);
        }

        internal bool IsBoldTagSymbol(string markdownText, int i) => 
            i + 1 < markdownText.Length && markdownText[i] == '_' && markdownText[i + 1] == '_';
        
        private StringBuilder CreateHtmlTag(string markdownText, int openTagIndex, int closingTagIndex)
        {
            var htmlTag = new StringBuilder(markdownText);

            htmlTag.Remove(closingTagIndex, 2);
            htmlTag.Insert(closingTagIndex, "</strong>");
            htmlTag.Remove(openTagIndex, 2);
            htmlTag.Insert(openTagIndex, "<strong>");

            return htmlTag;
        }

        private (StringBuilder tag, int index) FindClosingTagIndex(string markdownText, int openTagIndex)
        {
            var htmlTag = new StringBuilder(markdownText);
            //var nestedTadIndex = 0;

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (IsBoldTagSymbol(markdownText, i))
                    return (htmlTag, i);

                var newTag = TagFinder.FindTag(markdownText, i, settings);
                //   Tag tag1 = new Tag();

                if (newTag == null)
                    continue;

                if (newTag.Index == i)
                    continue;

                htmlTag = newTag.Text;
                i = newTag.Index;
                markdownText = htmlTag.ToString();

            }

            return (htmlTag, -1);
        }

        private (StringBuilder, int) ProcessAnotherTag(string markdownText, int i)
        {
            var italic = new ItalicHandler();
            var tag = italic.GetHtmlTag(markdownText, i);
            var htmlText = tag.Text;
            i = tag.Index;

            return (htmlText, i);
        }
    }
}