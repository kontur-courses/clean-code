using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Html;
using Markdown.Parsers.Tokens.Markdown;

namespace Markdown.Parsers.Tokens
{
    public class TextToken : Token
    {
        public TextToken(string data) : base(data)
        {
        }

        public override IToken ToHtml() => new HtmlTextToken(text);

        public override IToken ToMarkdown() => new MdTextToken(text);
    }
}
