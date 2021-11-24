namespace HtmlConvertor.ITokens
{
    public class ScreenerToken : IToken
    {
        public ScreenerToken(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}