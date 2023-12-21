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
            { TagType.Header, "<h1>" }
        };

        private static readonly Dictionary<TagType, string> endTags = new()
        {
            { TagType.Bold, "</strong>" },
            { TagType.Italic, "</em>" },
            { TagType.Header, "</h1>" }
        };

        public string InsertTags(ParsedText parsedText)
        {
            var sb = new StringBuilder();
            var prevTagPos = 0;
            foreach (var tag in parsedText.tags)
            {
                sb.Append(parsedText.paragraph.AsSpan(prevTagPos, tag.Position - prevTagPos));
                sb.Append(tag.IsEndTag ? endTags[tag.Type] : startTags[tag.Type]);
                prevTagPos = tag.Position;
            }

            sb.Append(parsedText.paragraph.AsSpan(prevTagPos, parsedText.paragraph.Length - prevTagPos));
            return sb.ToString();
        }
    }
}
