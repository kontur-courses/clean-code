namespace Markdown.Tokens
{
    public abstract class MdToken
    {
        public TokenType Type { get; protected set; }
    }
}