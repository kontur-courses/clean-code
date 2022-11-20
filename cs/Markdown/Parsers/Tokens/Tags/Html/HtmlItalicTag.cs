using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Markdown;

namespace Markdown.Parsers.Tokens.Tags.Html
{
    public class HtmlItalicTag : HtmlPairedTag
    {
        public HtmlItalicTag(TagPosition position) : base(position, "<em>")
        {
        }

        public override IToken ToMarkdown() => new MdItalicTag(position);
    }
}
