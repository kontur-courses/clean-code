namespace Markdown.Tokens;

public class ItalicToken : DoubleToken
{
    public ItalicToken(Token? parent = null) : base("_", "_", TokenType.Italic, parent) { }
}