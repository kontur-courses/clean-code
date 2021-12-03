namespace Markdown.Tokens
{
    public class PlainTextToken : MarkdownToken
    {
        public PlainTextToken(string value, string selector, int paragraphIndex, int startIndex) : base(value, selector, paragraphIndex, startIndex)
        {
        }

        public override string GetHtmlFormatted()
        {
            return Value;
        }
    }
}