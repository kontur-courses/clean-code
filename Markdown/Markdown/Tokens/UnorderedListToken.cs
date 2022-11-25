namespace Markdown.Tokens;

public class UnorderedListToken : MultilineToken
{
    public UnorderedListToken() 
        : base(string.Empty, string.Empty, TokenType.UnorderedList) { }
}