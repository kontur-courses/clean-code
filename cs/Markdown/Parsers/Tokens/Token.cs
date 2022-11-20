using System;

namespace Markdown.Parsers.Tokens
{
    public abstract class Token : IToken
    {
        protected readonly string text;

        protected Token(string data)
        {
            text = data;
        }

        public override string ToString() => text;

        public virtual IToken ToText() => new TextToken(text);

        public virtual IToken ToHtml()
        {
            throw new NotImplementedException();
        }


        public virtual IToken ToMarkdown()
        {
            throw new NotImplementedException();
        }
    }
}
