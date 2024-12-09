using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;
using Markdown.Tags.ConcreteTags;

namespace Markdown.Converter.ConcreteConverter
{
    public static class MdTagToHtmlConverter
    {
        public static Dictionary<TagType, string> OpenTags = new()
        {
            { TagType.Header, "<h1>"},
            { TagType.Bold, "<strong>"},
            { TagType.BulletedListItem, "<li>"},
            { TagType.Italic, "<em>"}
        };

        public static Dictionary<TagType, string> CloseTags = new()
        {
            { TagType.Header, "</h1>"},
            { TagType.Bold, "</strong>"},
            { TagType.BulletedListItem, "</li>"},
            { TagType.Italic, "</em>"}
        };
    }
}
