using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tags.Html;

namespace Markdown.Parsers.Tags.Markdown
{
    public class MdTextTag : Tag
    {
        public MdTextTag(string text) : base(text)
        {

        }

        public override ITag ToText() => new MdTextTag(text);

        public override ITag ToHtml() => new HtmlTextTag(text);
    }
}
