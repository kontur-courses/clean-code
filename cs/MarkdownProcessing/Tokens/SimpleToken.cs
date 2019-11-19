namespace MarkdownProcessing.Tokens
{
    public class SimpleToken : Token
    {
        public readonly string InnerText;

        public SimpleToken(string text)
        {
            Type = TokenType.PlainText;
            InnerText = text;
        }
    }
}