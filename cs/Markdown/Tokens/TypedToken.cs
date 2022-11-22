namespace Markdown.Tokens
{
    public class TypedToken : Token
    {
        public TokenType Type { get;}

        public TypedToken(int start, int length, TokenType type) 
            : base(start, length)
        {
            Type = type;
        }
    }
}
