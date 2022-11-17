using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tags.Enum;
using Markdown.Parsers.Tags.Html;

namespace Markdown.Parsers.Tags.Markdown
{
    public class MdItalicTag : MdTextTag
    {
        public MdItalicTag(TagPosition position) : base("_")
        {
            this.position = position;
        }

        public override ITag ToHtml() => new HtmlItalicTag(position);
    }
}
