using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tags.Enum;
using Markdown.Parsers.Tags.Markdown;

namespace Markdown.Parsers.Tags
{
    public abstract class Tag : ITag
    {
        protected TagPosition position;
        protected readonly string text;
        protected Tag(string data)
        {
            text = data;
            position = TagPosition.Any;
        }
        public override string ToString() => text;
        public virtual ITag ToText()
        {
            throw new NotImplementedException();
        }

        public virtual ITag ToHtml()
        {
            throw new NotImplementedException();
        }


        public virtual ITag ToMarkdown()
        {
            throw new NotImplementedException();
        }
    }
}
