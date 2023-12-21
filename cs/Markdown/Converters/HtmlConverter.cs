using System.Text;
using Markdown.Tags;

namespace Markdown.Converters
{
    public class HtmlConverter : IConverter
    {
        private static readonly Dictionary<TagType, string> startTags = new()
        {
            { TagType.Bold, "<strong>" },
            { TagType.Italic, "<em>" },
            { TagType.Header, "<h1>" },
            { TagType.BulletedList, "<li>"}
        };

        private static readonly Dictionary<TagType, string> endTags = new()
        {
            { TagType.Bold, "</strong>" },
            { TagType.Italic, "</em>" },
            { TagType.Header, "</h1>" },
            { TagType.BulletedList, "</li>"}
        };

        public string InsertTags(ParsedText[] parsedText)
        {
            var sb = new StringBuilder();
            var isList = false;
            foreach (var text in parsedText)
            {
                if (isList && text.tags.FirstOrDefault() is not BulletedListTag)
                {
                    isList = false;
                    sb.Append("</ul>");
                }
                else if (!isList && text.tags.FirstOrDefault() is BulletedListTag)
                {
                    isList = true;
                    sb.Append("<ul>");
                }
                var prevTagPos = 0;
                foreach (var tag in text.tags)
                {
                    sb.Append(text.paragraph.AsSpan(prevTagPos, tag.Position - prevTagPos));
                    sb.Append(tag.IsEndTag ? endTags[tag.Type] : startTags[tag.Type]);
                    prevTagPos = tag.Position;
                }

                sb.Append(text.paragraph.AsSpan(prevTagPos, text.paragraph.Length - prevTagPos));
            }
            if (isList)
                sb.Append("</ul>");
            return sb.ToString();
        }
    }
}
