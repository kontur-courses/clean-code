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

        public string ConvertMdToHtml(string mdText, List<Tag> tags)
        {
            PrepareToConvert();
            var htmlText = new StringBuilder();

            for (; currentPos < mdText.Length; currentPos++)
            {
                var tag = tags.FirstOrDefault(tag => tag.StartsAt == currentPos);

                if (tag == null)
                {
                    htmlText.Append(mdText[currentPos]);
                    continue;
                }

                htmlText.Append(GetHtmlTag(mdText, tag));
            }

            return htmlText.ToString();
        }

        private void PrepareToConvert()
        {
            currentPos = 0;
        }

        private string GetHtmlTag(string mdText, Tag tag)
        {
            var htmlTag = new StringBuilder();
            var htmlStyle = HtmlStyleKeeper.Styles[tag.TagStyleInfo.Type];
            var tagContent = GetTagContent(mdText, tag);
            htmlTag
                .Append(htmlStyle.TagPrefix)
                .Append(tagContent)
                .Append(htmlStyle.TagAffix);

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