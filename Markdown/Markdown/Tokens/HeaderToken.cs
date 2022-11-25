namespace Markdown.Tokens;

public class HeaderToken : LineToken
{
    public HeaderToken() : base(TokenType.Header, "# ", string.Empty) { }
}