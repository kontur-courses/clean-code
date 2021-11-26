namespace Markdown.Tokens
{
    public class PlainTextToken : Token, IMarkdownToken
    {
        public PlainTextToken(string value, int paragraphIndex, int startIndex) : base(value, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            throw new System.NotImplementedException();
        }
    }
}