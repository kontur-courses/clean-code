namespace Markdown
{
    public class MdBoldToken : BasicToken
    {
        public MdBoldToken() : this(0, 0, null)
        {
        }

        public MdBoldToken(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}