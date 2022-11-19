namespace Markdown.Token
{
    internal class Token : IToken
    {
        public string Value { get; }

        public Token(string value)
        {
            Value = value;
        }
    }
}
