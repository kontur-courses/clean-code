using System.Text;

namespace Markdown.TagHandlers
{
    public class HeadingHandler : IHtmlTagCreator
    {
        private FindTagSettings settings = new(true, true, true);
        public Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex, string? parentClosingTag)
        {
            var correct = IsCorrectOpenTag(markdownText, openTagIndex);

            if (!correct)
                return new Tag(markdownText, openTagIndex);

            var closingIndex = FindClosingTagIndex(markdownText, openTagIndex + 1);

            var tag = CreateHtmlTag(markdownText, openTagIndex, closingIndex.Index);
            var htmlTag = tag.Text;

            return new Tag(htmlTag, tag.Index);
        }

        private bool IsCorrectOpenTag(StringBuilder markdownText, int openTagIndex) =>
            openTagIndex == 0 || markdownText[openTagIndex - 1] == '\n';
        
        private Tag FindClosingTagIndex(StringBuilder markdownText, int openTagIndex)
        {
            var resultTag = new Tag(markdownText, -1);

            for (var i = openTagIndex; i < markdownText.Length; i++)
            {
                if (i + 1 >= markdownText.Length)
                    continue;

                if (markdownText[i] == '\n')
                {
                    resultTag.Index = i - 1;
                    return resultTag;
                }
            
                var newTag = TagFinder.FindTag(markdownText, i, settings, "#");

                if (newTag == null || newTag!.Text == null)
                    continue;
              
                resultTag.NestedTags.Add(newTag);

                i = newTag.Index;
            }

            return resultTag;
        }

        private Tag CreateHtmlTag(StringBuilder markdownText, int openTagIndex, int closingIndex)
        {
            markdownText.Insert(closingIndex == -1 ? markdownText.Length : closingIndex, "</h1>");
            markdownText.Remove(openTagIndex, 1);
            markdownText.Insert(openTagIndex, "<h1>");

            return new Tag(markdownText, closingIndex == -1 ? markdownText.Length : closingIndex);
        }

        public bool IsHeadingTagSymbol(StringBuilder markdownText, int i) => markdownText[i] == '#';
    }
}