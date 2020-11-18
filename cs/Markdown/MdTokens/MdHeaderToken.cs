namespace Markdown
{
    public class MdHeaderToken : TokenWithSubTokens
    {
        public MdHeaderToken() : this(0, 0, null)
        {
        }
        
        public MdHeaderToken(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}