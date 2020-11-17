using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class MarkdownToHtmlConverter
    {
        public static string Convert(string text, IEnumerable<Tag> tags)
        {
            var position = 0;
            var html = new StringBuilder();
            foreach (var tag in tags.OrderBy(x => x.Position))
            {
                html.Append(text.Substring(position, tag.Position - position));
                html.Append(tag.GetHtmlTag());
                position = tag.Position + tag.MdTag.Length;
            }

            html.Append(text.Substring(position, text.Length - position));
            return html.ToString();
        }
    }
}