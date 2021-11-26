namespace Markdown.Tokens
{
    public class ItalicToken : Token, IMarkdownToken
    {
        public ItalicToken(string value, int paragraphIndex, int startIndex) : base(value, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            throw new System.NotImplementedException();
        }
    }
}