namespace Markdown.Tokens
{
    public class EmphasizedTextToken : TextToken, ITagToken
    {
        public EmphasizedTextToken(string text)
            : base(TokenType.Emphasized, text)
        {
            TextWithoutTags = text[1..(text.Length - 1)];
        }

        public string TextWithoutTags { get; }
    }
}