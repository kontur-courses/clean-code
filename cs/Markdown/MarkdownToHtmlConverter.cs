using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class MarkdownToHtmlConverter
    {
        public static string Convert(string text, IEnumerable<Tag> tags)
        {
            var position = 0;
            var html = new StringBuilder();
            foreach (var tag in tags)
            {
                html.Append(text.Substring(position, tag.Position - position));
                html.Append(TagParser.SupportedTags[tag.Type].GetHtmlTag(tag.IsOpening));
                position = tag.Position + tag.MdTagLength;
            }

            html.Append(text.Substring(position, text.Length - position));
            return html.ToString();
        }
    }
}