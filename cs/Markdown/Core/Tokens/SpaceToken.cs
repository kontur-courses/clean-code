namespace Markdown.Core.Tokens
{
    public class SpaceToken : Token
    {
        public SpaceToken(int position, string value) : base(position, value, Tokens.TokenType.SpaceSymbol)
        {
        }
    }
}