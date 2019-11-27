namespace Markdown
{
    public class TokenBuilder
    {
        private readonly Token token = new Token();
        
        public TokenBuilder SetPosition(int position)
        {
            token.Position = position;
            return this;
        }

        public TokenBuilder SetMdTag(string mdTag)
        {
            token.MdTag = mdTag;
            return this;
        }

        public TokenBuilder SetHtmlTagName(string htmlTagName)
        {
            token.HtmlTagName = htmlTagName;
            return this;
        }

        public TokenBuilder SetData(string data)
        {
            token.Data = data;
            return this;
        }

        public TokenBuilder SetIsClosed(bool isClosed)
        {
            token.IsClosed = isClosed;
            return this;
        }

        public TokenBuilder SetIsValid(bool isValid)
        {
            token.IsValid = isValid;
            return this;
        }

        public TokenBuilder AddNestedToken(Token token)
        {
            this.token.AddNestedToken(token);
            return this;
        }
        
        public Token Build()
        {
            return token;
        }
    }
}