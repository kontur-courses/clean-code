namespace MarkdownConvertor.ITokens
{
    public class TextToken : IToken
    {
        public TextToken(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}