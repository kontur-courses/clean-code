namespace Markdown
{
    public class TemporaryToken : Token
    {
        public TemporaryToken(string value, int paragraphIndex, int startIndex) : base(value, paragraphIndex, startIndex)
        {
        }
    }
}