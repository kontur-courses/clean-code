namespace Markdown
{
    public class HeaderToken: Token, IMarkdownToken
    {
        public HeaderToken(string value, int paragraphIndex, int startIndex) : base(value, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            throw new System.NotImplementedException();
        }
    }
}