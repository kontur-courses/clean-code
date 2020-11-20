namespace Markdown
{
    public class RawTextToken : Token
    {
        public RawTextToken(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}