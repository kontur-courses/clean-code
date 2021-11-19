namespace Markdown.Tokens
{
    public class HtmlToken : IToken
    {
        public string Value { get; }

        public HtmlToken(string value)
        {
            Value = value;
        }
    }
}