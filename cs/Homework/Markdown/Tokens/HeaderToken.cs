namespace Markdown.Tokens
{
    public class HeaderToken: Token, IMarkdownToken
    {
        public HeaderToken(string value, string tag, int paragraphIndex, int startIndex) : base(value, tag, paragraphIndex, startIndex)
        {
        }

        public string GetHtmlFormatted()
        {
            var headerLevel = Tag.Length;
            var valueWithoutTag = Value[Tag.Length..].Trim();
            return $"<h{headerLevel}>{valueWithoutTag}</h{headerLevel}>";
        }

        
    }
}