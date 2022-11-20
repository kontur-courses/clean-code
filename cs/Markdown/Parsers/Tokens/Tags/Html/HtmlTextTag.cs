using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags;

namespace Markdown.Parsers.Tokens.Tags.Html
{
    public class HtmlTextTag : Tag
    {
        public HtmlTextTag(string text) : base(text)
        {
        }
    }
}
