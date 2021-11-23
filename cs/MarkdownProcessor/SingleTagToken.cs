namespace MarkdownProcessor
{
    public class SingleTagToken : IToken
    {
        public SingleTagToken(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}