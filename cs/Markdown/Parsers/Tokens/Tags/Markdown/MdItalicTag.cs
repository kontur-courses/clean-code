using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdItalicTag : PairedTag
    {
        public MdItalicTag(TagPosition position) : base(position, "_")
        {
        }

        public override IToken ToHtml() => new HtmlItalicTag(position);

        public override bool IsValidTag(string data, int position)
        {
            position++;
            return data.Length < position && char.IsLetter(data[position]);
        }
    }
}
