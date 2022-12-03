using System;
using Markdown.Parsers.Tokens.Tags.Markdown;

namespace Markdown.Parsers.Tokens.Tags
{
    public abstract class Tag : Token
    {
        protected Tag(string data) : base(data)
        {
        }

        public virtual bool IsValidTag(string currentLine, int position)
        {
            throw new NotImplementedException();
        }
    }
}
