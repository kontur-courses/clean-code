namespace Markdown
{
    public class MdBoldToken : MdTokenWithSubTokens
    {
        public MdBoldToken(int startPosition, int length = 0) : base(startPosition, length) { }
    }
}