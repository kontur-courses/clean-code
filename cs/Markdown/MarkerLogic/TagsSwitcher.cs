using System.Text;
using Markdown.Interfaces;
using Markdown.TagClasses;

namespace Markdown.MarkerLogic
{
    public class TagsSwitcher : ITagsSwitcher
    {
        public string SwitchTags(List<TagInfo> tags, string paragraph)
        {
            tags.Sort();

            var result = new StringBuilder(paragraph);
            foreach (var tag in tags)
            {
                SwitchTag(result, tag);
                AdjustPositions(tags, tag);
            }

            var header = tags.FirstOrDefault(x => !x.IsEscaped && x.Type == TagType.Header);
            if (header is not null)
            {
                result.Remove(0, 1);
                result.Insert(0, "<h1>");
                result.Append("</h1>");
            }

            result.Replace(@"\\", @"\");
            return result.ToString();
        }

        private static void AdjustPositions(IEnumerable<TagInfo> tags, TagInfo tag)
        {
            if (tag.IsEscaped)
            {
                tags.Where(x => x.Position > tag.Position).ToList().ForEach(x => x.Position--);
                return;
            }

            tags.Where(x => x.Position > tag.Position).ToList()
                .ForEach(x => x.Position += tag.GetHtmlTag().Length - tag.Length);
        }

        private static void SwitchTag(StringBuilder paragraph, TagInfo tag)
        {
            if (tag.IsEscaped)
            {
                paragraph.Remove(tag.Position - 1, 1);
                return;
            }

            paragraph.Remove(tag.Position, tag.Length);
            paragraph.Insert(tag.Position, tag.GetHtmlTag());
        }
    }
}