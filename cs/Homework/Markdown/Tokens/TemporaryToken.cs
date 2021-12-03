namespace Markdown.Tokens
{
    public class TemporaryToken : Token
    {
        public TemporaryToken(string value, string selector, int paragraphIndex, int startIndex) : base(value, selector, paragraphIndex, startIndex)
        {
        }
    }
}