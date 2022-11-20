using System;
using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags
{
    public abstract class PairedTag : Tag
    {
        protected TagPosition position;
        protected PairedTag(TagPosition position, string data) : base(data)
        {
            this.position = position;
        }
    }
}
