namespace Markdown
{
    public class MdItalicToken : TokenWithSubTokens
    {
        public MdItalicToken() : this(0, 0, null)
        {
        }

        public MdItalicToken(int startPosition, int length = 0, Token parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}