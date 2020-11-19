namespace Markdown.Tokens
{
    public class HeaderTextToken : TextToken, ITagToken
    {
        public HeaderTextToken(string text)
            : base(TokenType.Header, text)
        {
            TextWithoutTags = text[1..];
        }

        public string TextWithoutTags { get; }
    }
}