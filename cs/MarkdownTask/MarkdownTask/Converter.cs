using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class Converter
    {
        private int currentPos;
        private int openedHeaderTagsCount;

        public string ConvertMdToHtml(string mdText, List<Tag> tags, List<int> escapedChars)
        {
            PrepareToConvert();

            var htmlText = new StringBuilder();

            for (; currentPos < mdText.Length; currentPos++)
            {
                if (escapedChars.Contains(currentPos))
                    continue;

                var tag = tags.FirstOrDefault(tag => tag.StartsAt == currentPos);

                if (tag == null)
                {
                    htmlText = CloseHeaderOrAppendChar(mdText, htmlText);
                }

                else if (tag.TagTagStyleInfo.Type == TagType.Header)
                {
                    htmlText = OpenHeaderTag(htmlText, tag);
                }

                else
                {
                    htmlText.Append(GetHtmlTag(mdText, tag, escapedChars));
                    currentPos--;
                }
            }

            htmlText = CloseAllHeaders(htmlText);
            return htmlText.ToString();
        }

        private void PrepareToConvert()
        {
            currentPos = 0;
            openedHeaderTagsCount = 0;
        }

        private StringBuilder CloseHeaderOrAppendChar(string mdText, StringBuilder htmlText)
        {
            return IsHeaderClosing(mdText)
                ? AddNewLines(mdText, CloseAllHeaders(htmlText))
                : htmlText.Append(mdText[currentPos]);
        }

        private bool IsHeaderClosing(string mdText)
        {
            return mdText[currentPos] == '\n'
                   && currentPos + 1 < mdText.Length
                   && mdText[currentPos + 1] == '\n';
        }

        private StringBuilder OpenHeaderTag(StringBuilder htmlText, Tag tag)
        {
            htmlText.Append(HtmlStyleKeeper.Styles[tag.TagTagStyleInfo.Type].TagPrefix);
            currentPos += tag.TagTagStyleInfo.TagPrefix.Length - 1;
            openedHeaderTagsCount++;

            return htmlText;
        }

        private StringBuilder CloseAllHeaders(StringBuilder htmlText)
        {
            while (openedHeaderTagsCount > 0) htmlText = CloseLastOpenedHeader(htmlText);

            return htmlText;
        }

        private StringBuilder CloseLastOpenedHeader(StringBuilder htmlText)
        {
            if (openedHeaderTagsCount == 0) return htmlText;

            htmlText.Append(HtmlStyleKeeper.Styles[TagType.Header].TagAffix);
            openedHeaderTagsCount--;

            return htmlText;
        }

        private StringBuilder AddNewLines(string mdText, StringBuilder htmlText)
        {
            while (currentPos < mdText.Length && mdText[currentPos] == '\n')
            {
                htmlText.Append(mdText[currentPos]);
                currentPos++;
            }

            currentPos--;

            return htmlText;
        }

        private string GetHtmlTag(string mdText, Tag tag, List<int> escapedChars)
        {
            currentPos = tag.ContentStartsAt;

            var htmlTag = new StringBuilder();
            var htmlStyle = HtmlStyleKeeper.Styles[tag.TagTagStyleInfo.Type];
            var tagContent = GetTagContent(mdText, tag, escapedChars);
            htmlTag.Append(htmlStyle.TagPrefix).Append(tagContent).Append(htmlStyle.TagAffix);

            currentPos = tag.ContentStartsAt + tag.ContentLength + tag.TagTagStyleInfo.TagAffix.Length;

            return htmlTag.ToString();
        }

        private string GetTagContent(string mdText, Tag tag, List<int> escapedChars)
        {
            var content = new StringBuilder();
            for (; currentPos < tag.ContentStartsAt + tag.ContentLength; currentPos++)
            {
                if (escapedChars.Contains(currentPos))
                    continue;

                var innerTag = tag.NextTag?.StartsAt == currentPos ? tag.NextTag : null;
                if (innerTag == null)
                {
                    content.Append(mdText[currentPos]);
                    continue;
                }

                var innerTagContent = GetHtmlTag(mdText, innerTag, escapedChars);
                content.Append(innerTagContent);
                currentPos--;
            }

            return content.ToString();
        }
    }
}