namespace Markdown
{
    public class PlaintTextToken : Token
    {
        public PlaintTextToken(int position, string value, int endPosition)
            : base(position, value, endPosition, TokenType.PlainText, false)
        {
        }
    }
}