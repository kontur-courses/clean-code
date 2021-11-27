namespace Markdown.Tokens
{
    public class ItalicToken : Token, IMarkdownToken
    {
        public ItalicToken(string value, string tag, int paragraphIndex, int startIndex) : base(value, tag, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            var valueWithoutTags = Value.Trim('_');
            return $"<i>{valueWithoutTags}</i>";
        }
    }
}