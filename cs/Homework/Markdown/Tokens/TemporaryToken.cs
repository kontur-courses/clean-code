namespace Markdown.Tokens
{
    public class TemporaryToken : Token
    {
        public TemporaryToken(string value, string tag, int paragraphIndex, int startIndex) : base(value, tag, paragraphIndex, startIndex)
        {
        }
    }
}