using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public abstract class HtmlConverter : Converter
    {
        private static readonly Dictionary<TagType, string> startTags = new()
        {
            { TagType.Bold, "<strong>" },
            { TagType.Italic, "<em>" },
            { TagType.Header, "<h>" }
        };

        private static readonly Dictionary<TagType, string> endTags = new()
        {
            { TagType.Bold, "</strong>" },
            { TagType.Italic, "</em>" },
            { TagType.Header, "</h>" }
        };

        public static string InsertNewTags((string paragraph, List<ITag> tags) paragraphInfo)
        {
            var sb = new StringBuilder();
            var prevTagPos = 0;
            foreach (var tag in paragraphInfo.tags)
            {
                if (tag.Position > paragraphInfo.paragraph.Length)
                    throw new ArgumentException("Tag position cannot be outside paragraph", nameof(paragraphInfo.tags));
                sb.Append(paragraphInfo.paragraph.AsSpan(prevTagPos, tag.Position - prevTagPos));
                sb.Append(tag.IsEndTag ? endTags[tag.Type] : startTags[tag.Type]);
                prevTagPos = tag.Position;
            }

            sb.Append(paragraphInfo.paragraph.AsSpan(prevTagPos, paragraphInfo.paragraph.Length - prevTagPos));
            return sb.ToString();
        }
    }
}
