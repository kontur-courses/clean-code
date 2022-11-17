using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tags.Enum;

namespace Markdown.Parsers.Tags.Html
{
    public abstract class HtmlPairedTag : Tag
    {
        protected HtmlPairedTag(TagPosition position, string text) : base(text)
        {
            this.position = position;
        }

        public override string ToString() =>
            position == TagPosition.Start ? text : text.Insert(1, "/");
    }
}
