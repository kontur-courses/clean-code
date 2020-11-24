namespace Markdown.Tokens
{
    public class ImageToken : Token
    {
        public ImageToken(int position, string value, int endPosition)
            : base(position, value, endPosition, TokenType.Image, false)
        {
        }
    }
}