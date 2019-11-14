namespace Markdown.Core.Tokens
{
    public class TextToken : Token
    {
        public TextToken(int position, string value) : base(position, value, TokenType.Text)
        {
        }
    }
}