namespace Markdown
{
    public class HeaderParser : IParser
    {
        public IToken TryGetToken()
        {
            return new TagHeader();
        }
    }
}
