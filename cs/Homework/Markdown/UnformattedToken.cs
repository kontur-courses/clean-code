namespace Markdown
{
    public class UnformattedToken : Token, IMarkdownToken
    {
        public UnformattedToken(TokenType type, string value, int startIndex, int finishIndex) : base(type, value, startIndex, finishIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            throw new System.NotImplementedException();
        }
    }
}