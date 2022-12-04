using System;

namespace Markdown.Parsers.Tokens
{
    public abstract class Token : IToken
    {
        protected readonly string Text;

        protected Token(string data)
        {
            Text = data;
        }

        public override string ToString() => Text;

        public virtual IToken ToText() => new TextToken(Text);

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
