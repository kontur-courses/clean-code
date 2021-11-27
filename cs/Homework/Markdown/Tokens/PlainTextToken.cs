namespace Markdown.Tokens
{
    public class PlainTextToken : Token, IMarkdownToken
    {
        public PlainTextToken(string value, string tag, int paragraphIndex, int startIndex) : base(value, tag, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            return Value;
        }
    }
}