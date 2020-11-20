namespace Markdown
{
    public class BasicToken : TokenWithSubTokens
    {
        public BasicToken(int startPosition, int length = 0, Token parent = null) : base(startPosition, length, parent)
        {
        }
    }
}