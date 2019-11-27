namespace Markdown
{
    public class RootTokenBuilder
    {
        private readonly RootToken token = new RootToken();
        
        public RootTokenBuilder SetData(string data)
        {
            token.Data = data;
            return this;
        }

        public RootTokenBuilder AddNestedToken(Token token)
        {
            this.token.AddNestedToken(token);
            return this;
        }
        
        public RootToken Build()
        {
            return token;
        }
    }
}