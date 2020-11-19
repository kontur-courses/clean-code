namespace Markdown.Tokens
{
    public class StrongTextToken : TextToken, ITagToken
    {
        public StrongTextToken(string text)
            : base(TokenType.Strong, text)
        {
            TextWithoutTags = text[2..(text.Length - 2)];
        }

        public string TextWithoutTags { get; }
    }
}