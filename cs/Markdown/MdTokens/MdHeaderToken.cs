namespace Markdown
{
    public class MdHeaderToken : MdTokenWithSubTokens
    {
        public MdHeaderToken() : this(0, 0, null)
        {
        }
        
        public MdHeaderToken(int startPosition, int length = 0, MdToken parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}