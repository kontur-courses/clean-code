using System.Text;
using Markdown.ITagsInterfaces;
using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.MarkerLogic
{
    public class TagsSwitcher : ITagsSwitcher
    {
        public string SwitchTags(List<ITag> tags, string paragraph)
        {
            tags.Sort();
            var result = new StringBuilder(paragraph);

            foreach (var tag in tags)
            {
                SwitchTag(result, tag);
                AdjustPositions(tags, tag);
            }

            result.Replace(@"\\", @"\");
            return result.ToString();
        }

        private static void AdjustPositions(IEnumerable<ITag> tags, ITag tag)
        {
            if (tag.IsEscaped)
            {
                tags.Where(x => x.Position > tag.Position).ToList().ForEach(x => x.Position--);
                return;
            }

            tags.Where(x => x.Position > tag.Position).ToList()
                .ForEach(x => x.Position += tag.GetHtmlTag().Length - tag.Length);
        }

        private static void SwitchTag(StringBuilder paragraph, ITag tag)
        {
            if (tag.IsEscaped)
            {
                paragraph.Remove(tag.Position - 1, 1);
                return;
            }

            paragraph.Remove(tag.Position, tag.Length);
            paragraph.Insert(tag.Position, tag.GetHtmlTag());
            if (tag.Type is TagType.Header)
                paragraph.Append("</h1>");
        }
    }
}