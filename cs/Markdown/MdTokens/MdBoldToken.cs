namespace Markdown
{
    public class MdBoldToken : MdTokenWithSubTokens
    {
        public MdBoldToken() : this(0, 0, null)
        {
        }

        public MdBoldToken(int startPosition, int length = 0, MdToken parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}