namespace Markdown
{
    public class MdHeaderToken : MdTokenWithSubTokens
    {
        public MdHeaderToken(int startPosition, int length = 0) : base(startPosition, length) { }
    }
}