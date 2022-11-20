using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags.Html
{
    public class HtmlParagraphTag : PairedTag
    {
        public HtmlParagraphTag(TagPosition position) : base(position, "<p>")
        {
        }
    }
}
