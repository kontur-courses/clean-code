using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags.Html
{
    public class HtmlHeaderTag : HtmlPairedTag
    {
        public uint Level { get; private set; }
        public HtmlHeaderTag(TagPosition position, uint level = 1) : base(position, $"<h{level}>")
        {
            if (level == 0)
                throw new ArgumentException("level must be greater than zero");
            Level = level;
        }
    }
}
