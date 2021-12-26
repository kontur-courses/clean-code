using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class Converter
    {
        public string ConvertMdToHtml(string mdText, List<Tag> tags)
        {
            var stringBuilder = new StringBuilder();
            var htmlTags = GetHtmlTags(tags).ToList();

            return stringBuilder.ToString();
        }

        private IEnumerable<Tag> GetHtmlTags(IEnumerable<Tag> tags)
        {
            return tags.Select(tag =>
            {
                var htmlStyle = HtmlStyleKeeper.Styles[tag.TagStyleInfo.Type];
                return new Tag(
                    tag.StartsAt,
                    tag.StartsAt + htmlStyle.TagPrefix.Length,
                    tag.ContentLength,
                    htmlStyle.TagPrefix.Length + tag.ContentLength + htmlStyle.TagAffix.Length,
                    htmlStyle);
            });
        }
    }
}