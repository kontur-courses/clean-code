using System;
using Markdown.Parsers.Tokens.Tags.Markdown;

namespace Markdown.Parsers.Tokens.Tags
{
    public abstract class Tag : Token
    {
        protected Tag(string data) : base(data)
        {
        }

        //public virtual bool IsCommentedTag(IToken token)


        //TODO: проверка идет последнего символа, не нужно грузить всю строку
        public virtual bool IsValidTag(string data, int position)
        {
            throw new NotImplementedException();
        }
    }
}
