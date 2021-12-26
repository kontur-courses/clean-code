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

        public string ConvertMdToHtml(string mdText, List<Tag> tags)
        {
            PrepareToConvert();

            var htmlText = new StringBuilder();

            for (; currentPos < mdText.Length; currentPos++)
            {
                var tag = tags.FirstOrDefault(tag => tag.StartsAt == currentPos);

                if (tag == null)
                {
                    if (currentPos + 1 >= mdText.Length)
                    {
                        htmlText.Append(mdText[currentPos]);
                        htmlText = CloseAllHeaders(htmlText);
                    }

                    else if (mdText[currentPos] == '\n' && mdText[currentPos + 1] == '\n')
                    {
                        htmlText = CloseLastOpenedHeader(htmlText);
                        htmlText = AddNewLines(mdText, htmlText);
                    }

                    else
                    {
                        htmlText.Append(mdText[currentPos]);
                    }

                    continue;
                }

                if (tag.TagStyleInfo.Type == TagType.Header)
                {
                    htmlText.Append(HtmlStyleKeeper.Styles[tag.TagStyleInfo.Type].TagPrefix);
                    currentPos += tag.TagStyleInfo.TagPrefix.Length - 1;
                    openedHeaderTagsCount++;
                }
                else
                {
                    htmlText.Append(GetHtmlTag(mdText, tag));
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

        private StringBuilder CloseAllHeaders(StringBuilder htmlText)
        {
            while (openedHeaderTagsCount > 0)
                CloseLastOpenedHeader(htmlText);

            return htmlText;
        }

        private StringBuilder CloseLastOpenedHeader(StringBuilder htmlText)
        {
            if (openedHeaderTagsCount == 0)
                return htmlText;

            var htmlHeaderAffix = HtmlStyleKeeper.Styles[TagType.Header].TagAffix;
            htmlText.Append(htmlHeaderAffix);
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

        private string GetHtmlTag(string mdText, Tag tag)
        {
            var htmlTag = new StringBuilder();
            var htmlStyle = HtmlStyleKeeper.Styles[tag.TagStyleInfo.Type];
            var tagContent = GetTagContent(mdText, tag);

            htmlTag.Append(htmlStyle.TagPrefix).Append(tagContent).Append(htmlStyle.TagAffix);

            var tagStyleInfo = tag.TagStyleInfo;
            currentPos += tagStyleInfo.TagPrefix.Length +
                tagContent.Length +
                tagStyleInfo.TagAffix.Length - 1;

            return htmlTag.ToString();
        }

        private string GetTagContent(string mdText, Tag tag)
        {
            var contentBuilder = new StringBuilder();
            for (var i = tag.ContentStartsAt; i < tag.ContentStartsAt + tag.ContentLength; i++)
                contentBuilder.Append(mdText[i]);

            return contentBuilder.ToString();
        }
    }
}