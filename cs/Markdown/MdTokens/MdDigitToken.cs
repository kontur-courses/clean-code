namespace Markdown
{
    public class MdDigitToken : Token
    {
        public MdDigitToken() : this(0, 0, null)
        {
        }

        public MdDigitToken(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}