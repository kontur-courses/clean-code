using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public static class Md
    {
        private static readonly Dictionary<TagType, string> mdTags = new()
        {
            { TagType.Header, "# " },
            { TagType.Bold, "__" },
            { TagType.Italic, "_" },
            { TagType.Shield , "\\"}
        };

        public static string Render(string mdString) =>
            string.Join(
                "", mdString.ToParagraphs()
                    .Select(ParseParagraph)
                    .Select(HtmlConverter.InsertTags)
            );
        
        private static ParsedText ParseParagraph(string paragraph)
        {
            var tags = new List<ITag>();
            var position = 0;
            for (var i = IsHeaderParagraph(paragraph) ? 2 : 0; i < paragraph.Length; i++)
            {
                
            }
            
            paragraph = RemoveOldTags(paragraph, tags);
            if (IsHeaderParagraph(paragraph))
                tags.Add(new HeaderTag(position, true));
            return new ParsedText(paragraph, tags);
        }

        private static IEnumerable<string> ToParagraphs(this string text) => text.Split('\r', '\n');

        private static List<(ITag start, ITag end)> GetPairedTags(string paragraph)
        {
            throw new NotImplementedException();
        }
        
        private static bool IsHeaderParagraph(string paragraph) => paragraph.StartsWith("# ");
        
        private static string RemoveOldTags(string paragraph, IEnumerable<ITag> tags)
        {
            var stringBuilder = new StringBuilder();
            var prevPosition = 0;
            var tagShift = 0;
            foreach (var tag in tags)
            {
                stringBuilder.Append(paragraph.AsSpan(prevPosition, tag.Position + tagShift - prevPosition));
                prevPosition = tag.Position + tagShift;
                if (tag.IsShielded)
                    stringBuilder.Append(paragraph.AsSpan(prevPosition, mdTags[tag.Type].Length));
                tagShift += mdTags[tag.Type].Length;
                prevPosition += mdTags[tag.Type].Length;
            }

            stringBuilder.Append(paragraph.AsSpan(prevPosition, paragraph.Length - prevPosition));
            return stringBuilder.ToString();
        }
    }
}
