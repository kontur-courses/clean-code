using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tags.Enum;
using Markdown.Parsers.Tags.Markdown;

namespace Markdown.Parsers.Tags.Html
{
    public class HtmlItalicTag : HtmlPairedTag
    {
        public HtmlItalicTag(TagPosition position) : base(position, "<em>")
        {
        }

        public override ITag ToMarkdown() => new MdItalicTag(position);
    }
}
