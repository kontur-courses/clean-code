namespace Markdown.Tokens;

public class BoldToken : DoubleToken
{
    public BoldToken(Token? parent = null) : base("__", "__", TokenType.Bold, parent) { }
}