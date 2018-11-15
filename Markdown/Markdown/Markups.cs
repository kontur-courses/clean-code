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
            new Tag("strong", "<strong>", "</strong>"),
            new Tag("italic", "<em>", "</em>")
        });

        public static Markup Markdown { get; } = new Markup("HTML", new List<Tag>()
        {
            new Tag("strong", "__", "__", false),
            new Tag("italic", "_", "_")
        });
        
        public static Markup Rtf { get; } = new Markup("HTML", new List<Tag>()
        {
            new Tag("strong", "@{\b", "}"),
            new Tag("italic", @"{\i", "}")
        });
    }
}
