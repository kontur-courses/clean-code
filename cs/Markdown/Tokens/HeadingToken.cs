namespace Markdown.Tokens
{
    public class HeadingToken : Token
    {
        public HeadingToken(int position, string value, int endPosition)
            : base(position, value, endPosition, TokenType.Heading, true)
        {
        }
    }
}