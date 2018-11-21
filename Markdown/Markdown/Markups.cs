using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public sealed class Markups
    {
        public static Markup Html { get; } = new Markup("HTML", new List<Tag>()
        {
            new Tag(TagType.Strong, "<strong>", "</strong>",  new List<TagType> { TagType.Italic }),
            new Tag(TagType.Italic, "<em>", "</em>", new List<TagType> { TagType.Strong })
        });

        public static Markup Markdown { get; } = new Markup("Markdown", new List<Tag>()
        {
            new Tag(TagType.Strong, "__", "__"),
            new Tag(TagType.Italic, "_", "_", new List<TagType> { TagType.Strong })
        });
    }
}
