namespace Markdown.Tokens
{
    public class MarkdownToken : IToken
    {
        public string Value { get; }

        public MarkdownToken(string value)
        {
            Value = value;
        }
    }
}