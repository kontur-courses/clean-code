using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdTextTag : Tag
    {
        public MdTextTag(string text) : base(text)
        {

        }

        public override IToken ToHtml() => new HtmlTextTag(text);

        public override IToken ToMarkdown() => new MdTextTag(text);

        public override bool IsValidTag(string data, int position) => true;
    }
}
