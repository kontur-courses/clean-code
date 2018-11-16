using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public sealed class Markups
    {
        //properties
        public static Markup Html { get; } = new Markup("HTML", new List<Tag>()
        {
            new Tag(TagValue.Strong, "<strong>", "</strong>"),
            new Tag(TagValue.Italic, "<em>", "</em>")
        });

        public static Markup Markdown { get; } = new Markup("Markdown", new List<Tag>()
        {
            new Tag(TagValue.Strong, "__", "__", false),
            new Tag(TagValue.Italic, "_", "_")
        });
        
        public static Markup Rtf { get; } = new Markup("Rtf", new List<Tag>()
        {
            new Tag(TagValue.Strong, "@{\b", "}"),
            new Tag(TagValue.Italic, @"{\i", "}")
        });
    }
}
