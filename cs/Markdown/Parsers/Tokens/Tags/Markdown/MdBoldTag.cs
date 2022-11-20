using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdBoldTag : PairedTag
    {
        public MdBoldTag(TagPosition position) : base(position, "__")
        {
        }

        public override IToken ToHtml() => new HtmlBoldTag(position);

        public override bool IsValidTag(string data, int position)
        {
            position++;
            return data.Length < position && char.IsLetter(data[position]);
        }
    }
}
