using System;

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

        //TODO: проверка идет последнего символа, не нужно грузить всю строку
        public virtual bool IsValidTag(string data, int position)
        {
            throw new NotImplementedException();
        }
    }
}
