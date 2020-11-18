namespace Markdown
{
    public class MdItalicToken : MdTokenWithSubTokens
    {
        public MdItalicToken() : this(0, 0, null)
        {
        }

        public MdItalicToken(int startPosition, int length = 0, MdToken parent = null)
            : base(startPosition, length, parent)
        {
        }
    }
}