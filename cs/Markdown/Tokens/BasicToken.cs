namespace Markdown
{
    public abstract class BasicToken : TokenWithSubTokens
    {
        protected BasicToken(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}