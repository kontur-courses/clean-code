namespace Markdown.Tokens
{
    public class StrongToken : Token, IMarkdownToken
    {
        public StrongToken(string value, string tag, int paragraphIndex, int startIndex) : base(value, tag, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            var valueWithoutTags = Value.Trim('_');
            return $"<strong>{valueWithoutTags}</strong>";
        }
    }
}