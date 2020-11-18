namespace Markdown
{
    public class MdRawTextToken : Token
    {
        public MdRawTextToken(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}