using Markdown.Domain.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Config
{
    public class MarkdownConfig
    {
        public static Dictionary<Tag, string> HtmlTags => new Dictionary<Tag, string>()
        {
            { Tag.Bold, "strong" }, { Tag.Italic, "em" },
            { Tag.Header, "h1" }, { Tag.EscapedSymbol, "" }
        };

        public static Dictionary<Tag, string> MdTags => new Dictionary<Tag, string>()
        {
            { Tag.Bold, "__" }, { Tag.Italic, "_" },
            { Tag.Header, "# " }, { Tag.EscapedSymbol, ""}
        };

        public static Dictionary<Tag, Tag> DifferentTags => new Dictionary<Tag, Tag>()
        {
            {Tag.Bold, Tag.Italic},
            {Tag.Italic, Tag.Bold}
        };
    }
}
