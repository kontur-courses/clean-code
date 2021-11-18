namespace Markdown
{
    public class ItalicToken : Token, IMarkdownToken
    {
        public ItalicToken(TokenType type, string value, int startIndex, int finishIndex) : base(type, value, startIndex, finishIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            throw new System.NotImplementedException();
        }
    }
}