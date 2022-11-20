using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags
{
    public abstract class Tag : Token
    {
        protected Tag(string data) : base(data)
        {
        }
        public virtual bool IsCommentedTag(string data, int position)
        {
            return position > 0 && data[position - 1] == '\\';
        }

        public virtual bool IsValidTag(string data, int position)
        {
            throw new NotImplementedException();
        }
    }
}
