namespace Markdown.Tokens
{
    public class BoldToken : Token, IMarkdownToken
    {
        public BoldToken(string value, int paragraphIndex, int startIndex) : base(value, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            throw new System.NotImplementedException();
        }
    }
}